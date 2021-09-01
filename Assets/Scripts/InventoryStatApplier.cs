using System.Collections.Generic;
using UnityEngine;

public class InventoryStatApplier : MonoBehaviour
{
    private EquippedItemsHolder m_EquippedItems;
    private StatHolder m_StatHolder;
    private Dictionary<InventorySystem.InventorySlot, StatModifier> m_AppliedModifiers = new Dictionary<InventorySystem.InventorySlot, StatModifier>();

    private void Start()
    {
        m_StatHolder = GetComponent<StatHolder>();
        m_EquippedItems = GetComponent<EquippedItemsHolder>();
        m_EquippedItems.EquippedItemsInventory.ForEach(x => x.OnItemChange += OnItemChange);
    }

    private void OnDestroy()
    {
        m_EquippedItems.EquippedItemsInventory.ForEach(x => x.OnItemChange -= OnItemChange);
    }

    private void OnItemChange(InventorySystem.InventorySlot slot)
    {
        if (m_AppliedModifiers.ContainsKey(slot))
        {
            StatModifier oldModifier = m_AppliedModifiers[slot];
            if (oldModifier != null)
            {
                m_StatHolder.RemoveModifer(oldModifier);
            }
        }

        StatModifier newModifier = slot.Item != null ? slot.Item.GetDataComponent<StatModifierDataComponent>().Modifier : null;
        if (newModifier != null)
        {
            m_StatHolder.AddModifer(newModifier);
        }

        m_AppliedModifiers[slot] = newModifier;
    }
}
