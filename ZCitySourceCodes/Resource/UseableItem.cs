using System;

[Serializable]
public class UseableItem : Iitem, IInventory
{
    public UseableItemSO ItemData;

    public UseableItem(UseableItemSO data)
    {
        ItemData = data;
    }
    
    public void UseItem()
    {
        switch (ItemData.Variant)
        {
            case Define.ItemVariant.Food : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseHugner(ItemData.Value);
                break;
            case Define.ItemVariant.Water : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseWater(ItemData.Value);
                break;
            case Define.ItemVariant.FoodNWater : 
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseHugner(ItemData.Value);
                GameManager.Instance.player.GetComponent<PlayerCondition>().IncreaseWater(ItemData.Value);
                break;
            case Define.ItemVariant.Cure :
                GameManager.Instance.player.GetComponent<PlayerStatHandler>().IncreaseHP(ItemData.Value);
                break;
        }
    }

    public Define.ItemType GetItemType()
    {
        return ItemData.ItemType;
    }

    public ItemSO GetData()
    {
        return ItemData;
    }
}
