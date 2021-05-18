using UnityEngine;

public class InventoryHolder : MonoBehaviour
{
    [SerializeField]
    private InventoryChannel InventoryChannel;
    [SerializeField]
    private uint DefaultSlotCount = 0;
    [SerializeField]
    private bool CanCreateSlots = false;

    private InventorySystem.Inventory m_Inventory = new InventorySystem.Inventory();
    public InventorySystem.Inventory Inventory => m_Inventory;

    private void Awake()
    {
        InventoryChannel.OnInventoryItemLoot += OnInventoryItemLoot;
    }
    private void Start()
    {
        for (uint i = 0; i < DefaultSlotCount; ++i)
        {
            m_Inventory.CreateSlot();
        }
    }

    private void OnDestroy()
    {
        InventoryChannel.OnInventoryItemLoot -= OnInventoryItemLoot;
    }

    private void OnInventoryItemLoot(InventorySystem.InventoryItem item, uint quantity)
    {
        InventorySystem.InventorySlot slotToUse = m_Inventory.FindFirst(slot => slot.Item == item);
        if (slotToUse == null)
        {
            slotToUse = m_Inventory.FindFirst(slot => slot.Item == null);
        }

        if (slotToUse == null && CanCreateSlots)
        {
            slotToUse = m_Inventory.CreateSlot();
        }

        slotToUse?.StoreItem(item, quantity);
    }
}
