using System.Collections.Generic;

namespace InventorySystem
{
    public class FailedToMoveItemToSlotException : System.Exception
    {
    }

    public class InventorySlot
    {
        public delegate void ItemChangeCallback(InventorySlot slot);
        public ItemChangeCallback OnItemChange;

        private InventoryItem m_Item;
        private uint m_Quantity;
        private uint m_MaxQuantity = uint.MaxValue;
        private List<InventoryItemType> m_AllowedItemTypes = new List<InventoryItemType>();

        public InventoryItem Item => m_Item;
        public uint Quantity => m_Quantity;

        public uint MaxQuantity
        {
            get { return m_MaxQuantity; }
            set { m_MaxQuantity = value; }
        }

        public void AddAllowedItemType(InventoryItemType itemType)
        {
            m_AllowedItemTypes.Add(itemType);
        }

        public void StoreItem(InventoryItem item, uint quantity)
        {
            if ((m_Item == null || m_Item == item) && CanSlotContainItem(item) && CanAddItemsToSlot(quantity))
            {
                m_Item = item;
                m_Quantity += quantity;
                OnItemChange?.Invoke(this);
            }
            else
            {
                throw new FailedToMoveItemToSlotException();
            }
        }

        public void Clear()
        {
            m_Item = null;
            m_Quantity = 0;
            OnItemChange?.Invoke(this);
        }

        public void MoveTo(InventorySlot slotDestination, uint quantity)
        {
            if (slotDestination == null || quantity > m_Quantity || !slotDestination.CanSlotContainItem(m_Item))
            {
                throw new FailedToMoveItemToSlotException();
            }
            else
            {
                if (slotDestination.m_Item == m_Item || slotDestination.m_Item == null)
                {
                    uint movableQuantity = System.Math.Min(quantity, slotDestination.MaxQuantity - slotDestination.Quantity);
                    slotDestination.m_Item = m_Item;
                    slotDestination.m_Quantity += movableQuantity;
                    m_Quantity -= movableQuantity;

                    if (m_Quantity == 0)
                    {
                        Clear();
                    }
                }
                else if (m_Quantity == quantity)
                {
                    if (CanSlotHoldItems(slotDestination.m_Quantity) && slotDestination.CanSlotHoldItems(m_Quantity))
                    {
                        Utils.Swap(ref slotDestination.m_Item, ref m_Item);
                        Utils.Swap(ref slotDestination.m_Quantity, ref m_Quantity);
                    }
                    else
                    {
                        throw new FailedToMoveItemToSlotException();
                    }
                }
                else
                {
                    throw new FailedToMoveItemToSlotException();
                }
            }
            OnItemChange?.Invoke(this);
            slotDestination.OnItemChange?.Invoke(slotDestination);
        }

        public void MoveAllTo(InventorySlot slotDestination)
        {
            MoveTo(slotDestination, m_Quantity);
        }

        public bool CanSlotContainItem(InventoryItem item)
        {
            return item == null || CanSlotContainItemType(item.ItemType);
        }

        public bool CanSlotContainItemType(InventoryItemType itemType)
        {
            return m_AllowedItemTypes.Count == 0 || m_AllowedItemTypes.Contains(itemType);
        }

        public bool CanSlotHoldItems(uint quantity)
        {
            return quantity <= m_MaxQuantity;
        }

        public bool CanAddItemsToSlot(uint quantity)
        {
            return CanSlotHoldItems(m_Quantity + quantity);
        }
    }
}