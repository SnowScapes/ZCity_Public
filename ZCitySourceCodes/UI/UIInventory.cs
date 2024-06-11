using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public GameObject InventoryUIGameObject;
    public GameObject UseUIObject;
    public GameObject CraftUIObject;

    private Inventory playerInventory;

    [SerializeField] private List<InventorySlotUI> slots;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject useBtn;
    public GameObject dropBtn;

    [Header("Craft Slot")]
    public CraftSlotUI CraftSlot1;
    public CraftSlotUI CraftSlot2;
    public Image ResultSlot;
    public GameObject craftBtn;
    public GameObject tryBtn;
    
    public Crafting Craft;
    
    public int SeletedItemIndex { get; set; }
    

    public bool Mode { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameManager.Instance.player.GetComponent<Inventory>();
        Craft.ResetRecipe();
        ReloadInventory();
        InventoryUIGameObject.SetActive(false);
    }
    
    public void SetMode(bool mode)
    {
        Mode = mode;
        if (Mode)
        {
            UseUIObject.SetActive(true);
            CraftUIObject.SetActive(false);
        }
        else
        {
            UseUIObject.SetActive(false);
            CraftUIObject.SetActive(true);
        }
    }

    public void ReloadInventory()
    {
        for (int i = 0; i < playerInventory.PlayerItems.Count; i++)
        {
            slots[i].SetSlot();
        }

        if (gameObject.activeInHierarchy && slots.Count - playerInventory.PlayerItems.Count > 0)
        {
            for (int i = slots.Count - 1; i >= playerInventory.PlayerItems.Count; i--)
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void UseItem()
    {
        Iitem Useable = playerInventory.PlayerItems[SeletedItemIndex].itemData as Iitem;
        Useable.UseItem();
        playerInventory.SetAmount(SeletedItemIndex, -1);
        ReloadInventory();
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        useBtn.SetActive(false);
    }
    
    public void DropItem()
    {
        playerInventory.SetAmount(SeletedItemIndex, -1);
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        ReloadInventory();
    }

    public void DropCraftTable()
    {
        CraftSlot1.Icon.enabled = false;
        CraftSlot2.Icon.enabled = false;
        craftBtn.SetActive(false);
        Craft.ResetRecipe();
        tryBtn.SetActive(false);
    }

    public void TryCraft()
    {
        if (Craft.TryCrafting())
        {
            tryBtn.SetActive(false);
            craftBtn.SetActive(true);
            ResultSlot.enabled = true;
            ResultSlot.sprite = Craft.GetCraftInfo().Icon;
        }
    }

    public void ApplyCraft()
    {
        playerInventory.SetAmount(playerInventory.FindItem(CraftSlot1.itemID), -1);
        playerInventory.SetAmount(playerInventory.FindItem(CraftSlot2.itemID), -1);
        IInventory item = null;
        switch (Craft.GetCraftInfo().ItemType)
        {
            case Define.ItemType.UseableResource : 
                item= new UseableResource(Craft.GetCraftInfo() as UseableResourceSO);
                break;
            case Define.ItemType.UseableItem :
                item = new UseableItem(Craft.GetCraftInfo());
                break;
        }
        playerInventory.AddItem(new InventorySlot(item, 1));
        ResultSlot.enabled = false;
        DropCraftTable();
        ReloadInventory();
    }
}
