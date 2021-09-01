using System;
using System.Linq;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField]
    private InventoryUIChannel InventoryUIChannel;
    [SerializeField]
    private InventorySlotUIController SlotControllerPrefab;

    private InventorySystem.Inventory m_DisplayedInventory;
    public InventorySystem.Inventory DisplayedInventory => m_DisplayedInventory;

    private void Awake()
    {
        InventoryUIChannel.OnInventoryToggle += OnInventoryToggle;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InventoryUIChannel.OnInventoryToggle -= OnInventoryToggle;
    }

    private void OnInventoryToggle(InventoryHolder inventoryHolder)
    {
        if (m_DisplayedInventory == null)
        {
            gameObject.SetActive(true);
            m_DisplayedInventory = inventoryHolder.Inventory;
            m_DisplayedInventory.OnSlotAdded += CreateSlotController;
            m_DisplayedInventory.OnSlotRemoved += DestroyInventorySlot;
            m_DisplayedInventory.ForEach(CreateSlotController);
        }
        else if (m_DisplayedInventory == inventoryHolder.Inventory)
        {
            gameObject.SetActive(false);
            Array.ForEach(GetComponentsInChildren<InventorySlotUIController>(), slot => Destroy(slot.gameObject));
            m_DisplayedInventory.OnSlotRemoved -= DestroyInventorySlot;
            m_DisplayedInventory.OnSlotAdded -= CreateSlotController;
            m_DisplayedInventory = null;
        }
    }

    private void CreateSlotController(InventorySystem.InventorySlot slot)
    {
        InventorySlotUIController newSlotController = Instantiate(SlotControllerPrefab, transform);
        newSlotController.InventorySlot = slot;
    }

    private void DestroyInventorySlot(InventorySystem.InventorySlot slot)
    {
        InventorySlotUIController foundController = GetComponentsInChildren<InventorySlotUIController>().FirstOrDefault(slotController => slotController.InventorySlot == slot);
        if (foundController != null)
        {
            Destroy(foundController.gameObject);
        }
    }
}
