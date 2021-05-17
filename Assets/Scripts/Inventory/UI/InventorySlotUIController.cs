using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUIController : MonoBehaviour
{
    [SerializeField]
    private Text TextField;
    [SerializeField]
    private Text QuantityField;
    [SerializeField]
    private Image ImageField;

    private InventorySystem.InventorySlot m_InventorySlot;
    public InventorySystem.InventorySlot InventorySlot
    {
        get { return m_InventorySlot; }
        set
        {
            if (m_InventorySlot != null)
            {
                m_InventorySlot.OnItemChange -= UpdateSlot;
            }

            m_InventorySlot = value;
            UpdateSlot();

            if (m_InventorySlot != null)
            {
                m_InventorySlot.OnItemChange += UpdateSlot;
            }
        }
    }

    private void OnDestroy()
    {
        InventorySlot = null;
    }

    public void DestroySlot()
    {
        InventoryUIController inventory = GetComponentInParent<InventoryUIController>();
        if (inventory != null)
        {
            inventory.DisplayedInventory?.DestroySlot(m_InventorySlot);
        }
    }

    private void UpdateSlot()
    {
        bool displaySlot = m_InventorySlot != null && m_InventorySlot.Item != null;

        if (TextField != null)
        {
            TextField.gameObject.SetActive(displaySlot);
            TextField.text = (displaySlot ? m_InventorySlot.Item.Name : "");
        }

        if (QuantityField != null)
        {
            QuantityField.gameObject.SetActive(displaySlot);
            QuantityField.text = (displaySlot ? m_InventorySlot.Quantity.ToString() : "");
        }

        if (ImageField != null)
        {
            ImageField.gameObject.SetActive(displaySlot);
            ImageField.sprite = (displaySlot ? m_InventorySlot.Item.Sprite : null);
        }
    }
}
