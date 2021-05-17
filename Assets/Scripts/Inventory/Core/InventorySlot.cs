
namespace InventorySystem
{
    public class FailedToMoveItemToSlotException : System.Exception
    {
    }

    public class InventorySlot
    {
        public delegate void ItemChangeCallback();
        public ItemChangeCallback OnItemChange;

        public InventoryItem m_Item;
        public uint m_Quantity;

        public InventoryItem Item => m_Item;
        public uint Quantity => m_Quantity;

        public void StoreItem(InventoryItem item, uint quantity)
        {
            if (m_Item == null || m_Item == item)
            {
                m_Item = item;
                m_Quantity += quantity;
                OnItemChange?.Invoke();
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
            OnItemChange?.Invoke();
        }

        public void MoveTo(InventorySlot slotDestination, uint quantity)
        {
            if (slotDestination == null || quantity > m_Quantity)
            {
                throw new FailedToMoveItemToSlotException();
            }
            else
            {
                if (slotDestination.m_Item == m_Item || slotDestination.m_Item == null)
                {
                    slotDestination.m_Item = m_Item;
                    slotDestination.m_Quantity += quantity;
                    m_Quantity -= quantity;

                    if (m_Quantity == 0)
                    {
                        Clear();
                    }
                }
                else if (m_Quantity == quantity)
                {
                    Utils.Swap(ref slotDestination.m_Item, ref m_Item);
                    Utils.Swap(ref slotDestination.m_Quantity, ref m_Quantity);
                }
                else
                {
                    throw new FailedToMoveItemToSlotException();
                }
            }
            OnItemChange?.Invoke();
            slotDestination.OnItemChange?.Invoke();
        }

        public void MoveAllTo(InventorySlot slotDestination)
        {
            MoveTo(slotDestination, m_Quantity);
        }
    }
}