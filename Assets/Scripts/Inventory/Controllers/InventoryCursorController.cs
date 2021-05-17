using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCursorController : MonoBehaviour
{
    [SerializeField]
    private InventoryUIChannel InventoryUIChannel;
    [SerializeField]
    private InventoryChannel InventoryChannel;

    private InventorySystem.InventorySlot m_CursorSlot = new InventorySystem.InventorySlot();

    private void Awake()
    {
        InventoryUIChannel.OnInventoryToggle += OnInventoryToggle;
    }

    private void Start()
    {
        InventorySlotUIController slotController = GetComponent<InventorySlotUIController>();
        if (slotController != null)
        {
            slotController.InventorySlot = m_CursorSlot;
        }
    }

    private void OnDestroy()
    {
        InventoryUIChannel.OnInventoryToggle -= OnInventoryToggle;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InventorySystem.InventorySlot slot = FindSlotAtPosition();
            if (slot != null)
            {
                try
                {
                    m_CursorSlot.MoveAllTo(slot);
                }
                catch (InventorySystem.FailedToMoveItemToSlotException) {}
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            InventorySystem.InventorySlot slot = FindSlotAtPosition();
            if (slot != null && (slot.Item == m_CursorSlot.Item || m_CursorSlot.Item == null))
            {
                try
                {
                    slot.MoveTo(m_CursorSlot, 1);
                }
                catch (InventorySystem.FailedToMoveItemToSlotException) {}
            }
        }
    }

    private InventorySystem.InventorySlot FindSlotAtPosition()
    {
        InventorySystem.InventorySlot foundSlot = null;

        PointerEventData pointerEventData = new PointerEventData(null);
        pointerEventData.position = transform.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            InventorySlotUIController slotController = result.gameObject.GetComponent<InventorySlotUIController>();
            if (slotController != null)
            {
                foundSlot = slotController.InventorySlot;
                break;
            }
        }

        return foundSlot;
    }

    private void OnInventoryToggle(InventoryHolder inventoryHolder)
    {
        if (m_CursorSlot.Item != null)
        {
            InventoryChannel.RaiseLootItem(m_CursorSlot.Item, m_CursorSlot.Quantity);
            m_CursorSlot.Clear();
        }
    }
}
