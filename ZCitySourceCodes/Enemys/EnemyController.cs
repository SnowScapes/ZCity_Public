using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Running,
    Attacking,
    Stunned,
    Dead
}

public class EnemyController : MonoBehaviour, IDamage
{
    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRate;

    [Header("AI")]
    [SerializeField] private AIState aiState;

    [Header("Wandering")]
    [SerializeField] private float minWanderDistance;
    [SerializeField] private float maxWanderDistance;
    [SerializeField] private float minWanderWaitTime;
    [SerializeField] private float maxWanderWaitTime;

    [Header("Detection")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float fieldOfView;
    [SerializeField] private float losePlayerTime = 10f;

    [Header("Stun")]
    [SerializeField] private float stunDuration = 1f;

    private NavMeshAgent agent;
    private Animator animator;
    private GameObject player;
    private float lastPlayerSeenTime;
    private Vector3 originalPosition;
    private float lastAttackTime;
    private Collider enemyCollider;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<Collider>(); // 콜라이더 초기화
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalPosition = transform.position;
        SetState(AIState.Wandering);
    }

    private void Update()
    {
        switch (aiState)
        {
            case AIState.Idle:
                IdleUpdate();
                break;
            case AIState.Wandering:
                WanderingUpdate();
                DetectPlayer();
                break;
            case AIState.Running:
                RunningUpdate();
                DetectPlayer();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Stunned:
                break;
            case AIState.Dead:
                break;
        }

        if (aiState != AIState.Stunned && aiState != AIState.Dead)
        {
            animator.SetBool("Moving", !agent.isStopped && agent.remainingDistance > 0.1f);

            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                Vector3 direction = agent.velocity.normalized;
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            if (aiState == AIState.Running && Time.time - lastPlayerSeenTime >= losePlayerTime)
            {
                SetState(AIState.Wandering);
                WanderToNewLocation();
            }
        }
    }

    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.isStopped = false;
                agent.speed = walkSpeed;
                WanderToNewLocation();
                break;
            case AIState.Running:
                agent.isStopped = false;
                agent.speed = runSpeed;
                break;
            case AIState.Attacking:
                agent.isStopped = true;
                break;
            case AIState.Stunned:
                agent.isStopped = true;
                break;
            case AIState.Dead:
                agent.isStopped = true;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    private void IdleUpdate()
    {
    }

    private void WanderingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
    }

    private void RunningUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    private void AttackingUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            SetState(AIState.Running);
            return;
        }

        if (Time.time - lastAttackTime >= attackRate)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        AudioManager.Instance.PlayMonsterSFX(MonsterSfxSound.Attack);
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        IDamage playerDamage = player.GetComponent<IDamage>();
        if (playerDamage != null)
        {
            playerDamage.HandleDamage(damage);
        }
    }

    private void WanderToNewLocation()
    {
        if (aiState != AIState.Idle)
        {
            return;
        }

        SetState(AIState.Wandering);
        Vector3 wanderTarget = GetWanderLocation();
        agent.SetDestination(wanderTarget);
    }

    private Vector3 GetWanderLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maxWanderDistance, 1);
        Vector3 finalPosition = hit.position;

        return finalPosition;
    }

    private void DetectPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < detectionRange && IsPlayerInFieldOfView())
            {
                SetState(AIState.Running);
                agent.SetDestination(player.transform.position);
                lastPlayerSeenTime = Time.time;
            }
        }
    }

    private bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void HandleDamage(float damage)
    {
        AudioManager.Instance.PlayMonsterSFX(MonsterSfxSound.Hit);
        health -= (float)damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Stun();
        }
    }
    private void Stun()
    {
        SetState(AIState.Stunned);
        Invoke(nameof(RecoverFromStun), stunDuration);
    }

    private void RecoverFromStun()
    {
        if (aiState == AIState.Stunned)
        {
            SetState(AIState.Wandering);
        }
    }
    private void Die()
    {
        AudioManager.Instance.PlayMonsterSFX(MonsterSfxSound.Die);
        SetState(AIState.Dead);
        animator.SetTrigger("Die");
        enemyCollider.enabled = false; // 적이 죽으면 콜라이더 비활성화
        Invoke("DisableEnemy", 10f); // 10초 후에 DisableEnemy 메서드 호출
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false); // 적 객체 비활성화
    }


    // 적이 플레이어의 공격에 맞았을 때 호출되는 트리거 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            WeaponDamage weaponDamage = other.GetComponent<WeaponDamage>();
            if (weaponDamage != null)
            {
                float damage = weaponDamage.damage;
                HandleDamage((float)damage);
            }
        }
    }
}
