using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private PlayerStatHandler _statHandler;

    private float curMoveSpeed = 0f;
    private float rotationSpeed = 1f;
    private float interactionDistance = 2f;
    private Iinteraction _interaction;
    private Vector3 velocity;
    public Vector3 moveInput { get; set; }
    private Vector3 targetRot;
    private Vector3 aimPos;
    private bool AimMode = false;
    public bool SprintMode { get; set; } = false;
    private bool Interacting = false;
    private bool Attackable = true;

    [SerializeField] private LayerMask[] rootableMask;
    private int mask = 0;
    
    public event Action<float> MoveEvent;
    public event Action AttackEvent;
    public event Action<bool> AimEvent;
    
    private Iinteraction InteractTarget;
    private WaitForSeconds attackDelay;

    private void Start()
    {
        attackDelay = new WaitForSeconds(_statHandler.stat.AtkDelay);
        for (int i = 0; i < rootableMask.Length; i++)
            mask |= rootableMask[i].value;
    }

    private void Update()
    {
        Rotate();
        FindVisibleTarget();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        velocity = AimMode ? transform.forward * moveInput.z + transform.right * moveInput.x : transform.forward;
        if (moveInput.magnitude != 0)
            curMoveSpeed = SprintMode ? _statHandler.stat.SprintSpeed : _statHandler.stat.WalkSpeed;
        else
            curMoveSpeed = 0;
        velocity *= curMoveSpeed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
        CallMoveEvent(curMoveSpeed);
    }

    private void Rotate()
    {
        if (moveInput.magnitude > 0 && !AimMode)
        {
            if (Mathf.Abs(transform.eulerAngles.y - targetRot.y) < 180)
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, targetRot, rotationSpeed);
            else
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, targetRot, -rotationSpeed);
        }
        else if (AimMode)
        {
            transform.rotation = Quaternion.LookRotation(aimPos);
        }
    }

    private void CallMoveEvent(float speed)
    {
        MoveEvent?.Invoke(speed);
    }
    
    public void MoveAction(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector3>().normalized;
        if (moveInput.magnitude > 0f)
        {
            targetRot = Quaternion.LookRotation(moveInput).eulerAngles;
            targetRot.y += 45f;
        }
    }

    public void LookAction(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(context.ReadValue<Vector2>());
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            aimPos = hit.point;
            aimPos.y = transform.position.y;
            aimPos -= transform.position;
            aimPos = aimPos.normalized;
        }
    }

    public void InteractAction(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (context.phase == InputActionPhase.Performed)
        {
            StartCoroutine(RayCoroutine());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (UIManager.Instance.InteractUI.activeInHierarchy)
                UIManager.Instance.InteractUI.SetActive(false);
            Interacting = false;
        }
    }
    
    public void AttackAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && Attackable)
        {
            AttackEvent?.Invoke();
            StartCoroutine(AttackDelay());
        }
    }

    public void SprintAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && moveInput.magnitude != 0)
            SprintMode = true;
        else if (context.phase == InputActionPhase.Canceled)
            SprintMode = false;
    }

    public void InventoryAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            UIManager.Instance.InventoryUI.InventoryUIGameObject.SetActive(!UIManager.Instance.InventoryUI.InventoryUIGameObject.activeInHierarchy);
            UIManager.Instance.InventoryUI.SetMode(true);
        }
    }

    public void CraftAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            UIManager.Instance.InventoryUI.InventoryUIGameObject.SetActive(!UIManager.Instance.InventoryUI.InventoryUIGameObject.activeInHierarchy);
            UIManager.Instance.InventoryUI.SetMode(false);
        }
    }

    public void AimAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            AimMode = true;
            AimEvent?.Invoke(AimMode);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            AimMode = false;
            AimEvent?.Invoke(AimMode);
        }
    }

    private void FindVisibleTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, _statHandler.stat.ViewRange + 0.5f, LayerMask.GetMask("Zombie"));
        
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 direction = (targets[i].transform.position - transform.position).normalized;
            float distance = (targets[i].transform.position - transform.position).magnitude;
            EnemyVisibility enemy = targets[i].GetComponent<EnemyVisibility>();
            if ((Vector3.Angle(transform.forward, direction) < _statHandler.stat.FieldOfView / 2 && distance <= _statHandler.stat.ViewRange) || distance < _statHandler.stat.AroundViewRange)
            {
                enemy.InSight = true;
            }
            else
            {
                enemy.InSight = false;
            }
        }
    }

    

    private IEnumerator RayCoroutine()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,0.8f,0), transform.forward, out hit, interactionDistance))
        {
            if ((mask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                if (hit.collider.TryGetComponent<Iinteraction>(out Iinteraction rootable) || (rootable = hit.collider.GetComponentInParent<Iinteraction>()) != null)
                {
                    switch (hit.collider.gameObject.layer)
                    {
                        case 6:
                            AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.CollectItem);
                            break;
                        case 7:
                            AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.CollectWood);
                            break;
                        case 8:
                            AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.Collectstone);
                            break;
                        case 9:
                            AudioManager.Instance.PlayPlayerSFX(PlayerSfxSound.CollectBuilding);
                            break;
                    }
                    
                    Interacting = true;
                    InteractTarget = rootable;
                    StartCoroutine(InteractionCoroutine());
                }
            }
        }

        while (Interacting)
        {
            RaycastHit hitting;
            if (Physics.Raycast(transform.position + new Vector3(0,0.8f,0), transform.forward, out hitting, interactionDistance))
            {
                if (hitting.collider != hit.collider)
                    Interacting = false;
            }
            else
            {
                Interacting = false;
            }
            
            yield return null;
        }
    }

    private IEnumerator InteractionCoroutine()
    {
        while (true)
        {
            if (Interacting)
            {
                InteractTarget.Interaction();
            }
            else
            {
                InteractTarget.CancelInteraction();
                InteractTarget = null;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator AttackDelay()
    {
        Attackable = false;
        yield return attackDelay;
        Attackable = true;
    }
}
