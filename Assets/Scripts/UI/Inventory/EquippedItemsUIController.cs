using System;
using UnityEngine;

[Serializable]
public class EquippedItemsUISlot
{
    public InventorySlotUIController SlotController;
    public InventorySystem.InventoryItemType ItemType;
}

public class EquippedItemsUIController : MonoBehaviour
{
    [SerializeField]
    private InventoryUIChannel InventoryUIChannel;
    [SerializeField]
    private InventoryCursorController m_Cursor;
    [SerializeField]
    private EquippedItemsUISlot[] m_Slots;

    private InventorySystem.Inventory m_DisplayedEquippedItemsInventory;

    private void Awake()
    {
        InventoryUIChannel.OnInventoryToggle += OnInventoryToggle;
        gameObject.SetActive(false);
        m_Cursor.CursorSlot.OnItemChange += OnCursorItemChange;
    }

    private void OnDestroy()
    {
        InventoryUIChannel.OnInventoryToggle -= OnInventoryToggle;
        m_Cursor.CursorSlot.OnItemChange -= OnCursorItemChange;
    }

    private void OnInventoryToggle(InventoryHolder inventoryHolder)
    {
        InventorySystem.Inventory EquippedItemInventory = inventoryHolder.GetComponent<EquippedItemsHolder>().EquippedItemsInventory;

        if (m_DisplayedEquippedItemsInventory == null)
        {
            gameObject.SetActive(true);

            m_DisplayedEquippedItemsInventory = EquippedItemInventory;

            if (m_DisplayedEquippedItemsInventory != null)
            {
                foreach (EquippedItemsUISlot slot in m_Slots)
                {
                    slot.SlotController.InventorySlot = m_DisplayedEquippedItemsInventory.FindFirst(x => x.CanSlotContainItemType(slot.ItemType));
                }
            }
        }
        else if (m_DisplayedEquippedItemsInventory == EquippedItemInventory)
        {
            gameObject.SetActive(false);
            m_DisplayedEquippedItemsInventory = null;
            Array.ForEach(m_Slots, x => x.SlotController.InventorySlot = null);
        }
    }

    private void OnCursorItemChange(InventorySystem.InventorySlot slot)
    {
        InventorySystem.InventoryItem cursorItem = m_Cursor.CursorSlot.Item;
        foreach (EquippedItemsUISlot equipmentSlot in m_Slots)
        {
            equipmentSlot.SlotController.IsHighlighted = cursorItem != null && equipmentSlot.SlotController.InventorySlot.CanSlotContainItem(cursorItem);
        }
    }
}
