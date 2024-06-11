using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] public UIInventory InventoryUI;
    [SerializeField] public GameObject InteractUI;
    [SerializeField] public Image InteractUIFill;
    [SerializeField] public GameObject GameOverText;
}
