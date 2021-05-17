using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventoryChannel")]
public class InventoryChannel : ScriptableObject
{
    public delegate void InventoryItemLootCallback(InventorySystem.InventoryItem item, uint quantity);

    public InventoryItemLootCallback OnInventoryItemLoot;

    public void RaiseLootItem(InventorySystem.InventoryItem item)
    {
        OnInventoryItemLoot?.Invoke(item, 1);
    }

    public void RaiseLootItem(InventorySystem.InventoryItem item, uint quantity)
    {
        OnInventoryItemLoot?.Invoke(item, quantity);
    }
}
