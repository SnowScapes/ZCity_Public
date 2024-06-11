using UnityEngine;
using UnityEngine.UI;

public class ItemSO : ScriptableObject
{
    public Define.ItemType ItemType;
    public int ItemID;
    public string ItemName;
    public string Description;
    public Sprite Icon;
}
