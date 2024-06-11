public class InventorySlot
{
    public IInventory itemData;
    public int Amount { get; set; }

    public InventorySlot(IInventory data, int _amount)
    {
        itemData = data;
        Amount = _amount;
    }
}
    