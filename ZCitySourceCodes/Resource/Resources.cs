using System;

[Serializable]
public class Resources : ICraftResource, IInventory
{
    public ResourceSO ResData;

    public Resources(ResourceSO data)
    {
        ResData = data;
    }
    
    public int UseResource()
    {
        return ResData.ResourceID;
    }

    public Define.ItemType GetItemType()
    {
        return ResData.ItemType;
    }

    public ItemSO GetData()
    {
        return ResData;
    }
}
