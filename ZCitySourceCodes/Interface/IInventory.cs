using UnityEngine.UI;

public interface IInventory
{
    Define.ItemType GetItemType();
    ItemSO GetData();
}
