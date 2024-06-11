using UnityEngine;

public class RootableBuilding : RootableObject
{
    [SerializeField] private bool coolDown = true;
    public bool CoolDown
    {
        get
        {
            return coolDown;
        }
        set
        {
            coolDown = value;
            if(!coolDown)
                base.OnEnable();
        }
    }

    private WaitForSeconds delay;

    public override void Interaction()
    {
        if (!CoolDown)
        {
            if (CurInteractTime <= MaxInteractTime)
            {
                UIManager.Instance.InteractUI.SetActive(true);
                UIManager.Instance.InteractUIFill.fillAmount = CurInteractTime / MaxInteractTime;
                CurInteractTime += Time.deltaTime;
            }
            else
            {
                UIManager.Instance.InteractUIFill.fillAmount = 0;
                UIManager.Instance.InteractUI.SetActive(false);
                InventorySet();
                UIManager.Instance.InventoryUI.ReloadInventory();
                CoolDown = true;
            }
        }
    }
}
