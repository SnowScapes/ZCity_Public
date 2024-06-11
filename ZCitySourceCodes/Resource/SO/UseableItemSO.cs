using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ItemSO/UseableItemSO", order = 1)]
public class UseableItemSO : ItemSO
{
    public Define.ItemVariant Variant;
    public int Recipe;
    public int Value;
}