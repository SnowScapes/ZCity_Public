using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    private List<InventorySlot> playerInventory;
    private UIInventory inventory;

    [SerializeField] private int index;

    public Button Button;
    public Image IconImage;
    public TextMeshProUGUI AmountText;

    private void Start()
    {
        playerInventory = GameManager.Instance.player.GetComponent<Inventory>().PlayerItems;
        inventory = UIManager.Instance.InventoryUI;
    }

    public void SetSlot()
    {
        Button.enabled = true;
        IconImage.enabled = true;
        IconImage.sprite = playerInventory[index].itemData.GetData().Icon;
        AmountText.text = playerInventory[index].Amount.ToString();
    }

    public void SetEmpty()
    {
        Button.enabled = false;
        IconImage.enabled = false;
        AmountText.text = "";
    }
    
    public void SelectItem()
    {
        if (inventory.Mode)
        {
            inventory.selectedItemName.text = playerInventory[index].itemData.GetData().ItemName;
            inventory.selectedItemDescription.text = playerInventory[index].itemData.GetData().Description;
            inventory.SeletedItemIndex = index;
            if (playerInventory[index].itemData as Iitem != null)
            {
                inventory.useBtn.SetActive(true);
            }
            else
            {
                inventory.useBtn.SetActive(false);
            }
        }
        else
        {
            ICraftResource resource = playerInventory[index].itemData as ICraftResource;
            Debug.Log(resource);
            if (resource != null)
            {
                if (!inventory.CraftSlot1.Icon.enabled)
                {
                    inventory.CraftSlot1.itemID = playerInventory[index].itemData.GetData().ItemID;
                    inventory.CraftSlot1.Icon.enabled = true;
                    inventory.CraftSlot1.Icon.sprite = playerInventory[index].itemData.GetData().Icon;
                    inventory.Craft.AddResource(resource.UseResource());
                }
                else if (inventory.CraftSlot1.Icon.enabled && !inventory.CraftSlot2.Icon.enabled)
                {
                    inventory.CraftSlot2.itemID = playerInventory[index].itemData.GetData().ItemID;
                    inventory.CraftSlot2.Icon.enabled = true;
                    inventory.CraftSlot2.Icon.sprite = playerInventory[index].itemData.GetData().Icon;
                    inventory.Craft.AddResource(resource.UseResource());
                    inventory.tryBtn.SetActive(true);
                }
            }
        }
    }
}
