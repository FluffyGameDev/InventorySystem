using UnityEngine;

public class EquippedItemsHolder : MonoBehaviour
{
    [SerializeField]
    InventorySystem.InventoryItemType[] m_SlotItemTypes;

    private InventorySystem.Inventory m_EquippedItemsInventory = new InventorySystem.Inventory();
    public InventorySystem.Inventory EquippedItemsInventory => m_EquippedItemsInventory;

    private void Awake()
    {
        foreach (InventorySystem.InventoryItemType itemType in m_SlotItemTypes)
        {
            InventorySystem.InventorySlot newSlot = m_EquippedItemsInventory.CreateSlot();
            newSlot.MaxQuantity = 1;
            newSlot.AddAllowedItemType(itemType);
        }
    }
}
