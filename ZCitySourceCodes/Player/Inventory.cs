using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> PlayerItems = new List<InventorySlot>();

    private void Start()
    {
        InitInventory();
    }

    public int FindItem(int itemID)
    {
        int exist = -1;
        for (int i = 0; i < PlayerItems.Count; i++)
        {
            if (PlayerItems[i].itemData.GetData().ItemID == itemID)
                exist = i;
        }
        return exist;
    }

    public void AddItem(InventorySlot newItem)
    {
        PlayerItems.Add(newItem);
    }

    public void SetAmount(int index, int amount)
    {
        PlayerItems[index].Amount += amount;
        if(PlayerItems[index].Amount <= 0)
            PlayerItems.RemoveAt(index);
    }

    private void InitInventory()
    {
        PlayerItems.Clear();
    }
}
