using InventorySystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest
{
    [Test]
    public void CreateSlots()
    {
        const uint slotCount = 10;

        Inventory inventory = new Inventory();

        for (uint i = 0; i < slotCount; ++i)
        {
            inventory.CreateSlot();
        }

        Assert.AreEqual(inventory.SlotCount, slotCount);
    }

    [Test]
    public void ClearSlots()
    {
        const uint slotCount = 10;

        InventoryItem testItem = ScriptableObject.CreateInstance<InventoryItem>();

        Inventory inventory = new Inventory();

        for (uint i = 0; i < slotCount; ++i)
        {
            InventorySlot slot = inventory.CreateSlot();
            slot.StoreItem(testItem, 1);
        }

        inventory.ForEach(slot =>
        {
            Assert.AreEqual(slot.Item, testItem);
            Assert.AreEqual(slot.Quantity, 1);
        });

        inventory.Clear();

        Assert.AreEqual(inventory.SlotCount, slotCount);

        inventory.ForEach(slot =>
        {
            Assert.AreEqual(slot.Item, null);
            Assert.AreEqual(slot.Quantity, 0);
        });
    }

    [Test]
    public void FindFirstSlot()
    {
        const uint slotCount = 10;

        InventoryItem testItem1 = ScriptableObject.CreateInstance<InventoryItem>();
        InventoryItem testItem2 = ScriptableObject.CreateInstance<InventoryItem>();

        Inventory inventory = new Inventory();

        for (uint i = 0; i < slotCount; ++i)
        {
            InventorySlot slot = inventory.CreateSlot();
            slot.StoreItem(i == 7 ? testItem1 : testItem2, i + 1);
        }

        InventorySlot foundSlot = inventory.FindFirst(slot => slot.Item == testItem1);
        Assert.IsNotNull(foundSlot);
        Assert.AreEqual(foundSlot.Quantity, 8);
    }

    [Test]
    public void FindAllSlots()
    {
        const uint slotCount = 10;

        InventoryItem testItem1 = ScriptableObject.CreateInstance<InventoryItem>();
        InventoryItem testItem2 = ScriptableObject.CreateInstance<InventoryItem>();

        Inventory inventory = new Inventory();

        for (uint i = 0; i < slotCount; ++i)
        {
            InventorySlot slot = inventory.CreateSlot();
            slot.StoreItem((i % 3) == 0 ? testItem1 : testItem2, i + 1);
        }

        List<InventorySlot> foundSlot = inventory.FindAll(slot => slot.Item == testItem1);
        Assert.AreEqual(foundSlot.Count, 4);
    }
}