using UnityEngine;

public class UseableResource : Iitem, IInventory, ICraftResource
{
    public UseableResourceSO UrData;

    public UseableResource(UseableResourceSO data)
    {
        UrData = data;
    }
    
    public void UseItem()
    {
        Debug.Log(UrData);
        switch (UrData.Variant)
        {
            case Define.ItemVariant.Food : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseHugner(UrData.Value);
                break;
            case Define.ItemVariant.Water : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseWater(UrData.Value);
                break;
            case Define.ItemVariant.FoodNWater : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseHugner(UrData.Value);
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseWater(UrData.Value);
                break;
            case Define.ItemVariant.Cure :
                GameManager.Instance.player.GetComponent<PlayerStatHandler>().IncreaseHP(UrData.Value);
                break;
        }
    }

    public Define.ItemType GetItemType()
    {
        return UrData.ItemType;
    }

    public ItemSO GetData()
    {
        return UrData;
    }

    public int UseResource()
    {
        return UrData.ResourceID;
    }
}
