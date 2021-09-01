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
    [SerializeField]
    private Sprite EmptySlotSprite;
    [SerializeField]
    private Color HighlightColor;

    private Image m_Image;

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
            UpdateSlot(m_InventorySlot);

            if (m_InventorySlot != null)
            {
                m_InventorySlot.OnItemChange += UpdateSlot;
            }
        }
    }

    private Color m_DefaultColor;
    private bool m_IsHighlighted = false;
    public bool IsHighlighted
    {
        get { return m_IsHighlighted; }
        set
        {
            m_IsHighlighted = value;

            if (m_Image != null)
            {
                m_Image.color = (m_IsHighlighted ? HighlightColor : m_DefaultColor);
            }
        }
    }

    private void Start()
    {
        m_Image = GetComponent<Image>();
        if (m_Image != null)
        {
            m_DefaultColor = m_Image.color;
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

    private void UpdateSlot(InventorySystem.InventorySlot slot)
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
            ImageField.gameObject.SetActive(displaySlot || EmptySlotSprite != null);
            ImageField.sprite = (displaySlot ? m_InventorySlot.Item.Sprite : EmptySlotSprite);
        }
    }
}
