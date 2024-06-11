using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RootableObject : MonoBehaviour, Iinteraction
{
    [SerializeField] protected List<ItemSO> ResourcesList;
    protected int[] randomAmount;
    [SerializeField] protected int maxRandomAmount;
    protected int curRandomAmount;

    [SerializeField] public float MaxInteractTime;
    [SerializeField] public float CurInteractTime = 0f;

    protected Inventory playerInventory;

    protected virtual void Awake()
    {
        randomAmount = new int[ResourcesList.Count];
    }

    protected void Start()
    {
        playerInventory = GameManager.Instance.player.GetComponent<Inventory>();
    }

    protected virtual void OnEnable()
    {
        CurInteractTime = 0f;
        curRandomAmount = Random.Range(1, maxRandomAmount+1);
        RandomAmount();
    }

    private void RandomAmount()
    {
        for (int i = 0; i < ResourcesList.Count; i++)
        {
            randomAmount[i] = Random.Range(0, curRandomAmount+1);
        }

        if (randomAmount.Sum() == 0)
        {
            randomAmount[Random.Range(0, randomAmount.Length)] += 1;
        }

        while (randomAmount.Sum() > curRandomAmount)
        {
            int index = Random.Range(0, randomAmount.Length);
            if (randomAmount[index] > 0)
                randomAmount[index] -= 1;
        }
    }
    
    public virtual void Interaction()
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
            gameObject.SetActive(false);
        }
    }

    protected void InventorySet()
    {
        for (int i = 0; i < ResourcesList.Count; i++)
        {
            if (randomAmount[i] != 0)
            {
                int findIndex = playerInventory.FindItem(ResourcesList[i].ItemID);
            
                if (findIndex != -1)
                {
                    playerInventory.SetAmount(findIndex, randomAmount[i]);
                }
                else
                {
                    IInventory item = null;
                    switch (ResourcesList[i].ItemType)
                    {
                        case Define.ItemType.Resource : item = new Resources(ResourcesList[i] as ResourceSO);
                            break;
                        case Define.ItemType.UseableResource : item = new UseableResource(ResourcesList[i] as UseableResourceSO);
                            break;
                        case Define.ItemType.UseableItem : item = new UseableItem(ResourcesList[i] as UseableItemSO);
                            break;
                    }
                    playerInventory.AddItem(new InventorySlot(item, randomAmount[i]));
                }
            }
        }
    }
    
    public void CancelInteraction()
    {
        CurInteractTime = 0f;
    }
}
