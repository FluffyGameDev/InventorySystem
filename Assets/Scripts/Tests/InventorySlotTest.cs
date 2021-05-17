using InventorySystem;
using NUnit.Framework;
using UnityEngine;

public class InventorySlotTest
{

    [Test]
    public void StoreAndClearSlot()
    {
        InventoryItem testItem = ScriptableObject.CreateInstance<InventoryItem>();

        InventorySlot slot = new InventorySlot();

        slot.StoreItem(testItem, 10);

        Assert.AreEqual(slot.Item, testItem);
        Assert.AreEqual(slot.Quantity, 10);

        slot.Clear();

        Assert.AreEqual(slot.Item, null);
        Assert.AreEqual(slot.Quantity, 0);
    }

    [Test]
    public void MoveToEmptySlot()
    {
        InventoryItem testItem = ScriptableObject.CreateInstance<InventoryItem>();

        InventorySlot slotSource = new InventorySlot();
        InventorySlot slotDestination = new InventorySlot();

        slotSource.StoreItem(testItem, 10);

        slotSource.MoveTo(slotDestination, 4);


        Assert.AreEqual(slotSource.Item, testItem);
        Assert.AreEqual(slotSource.Quantity, 6);
        Assert.AreEqual(slotDestination.Item, testItem);
        Assert.AreEqual(slotDestination.Quantity, 4);
    }

    [Test]
    public void MoveError()
    {
        InventoryItem testItem1 = ScriptableObject.CreateInstance<InventoryItem>();
        InventoryItem testItem2 = ScriptableObject.CreateInstance<InventoryItem>();

        InventorySlot slotSource = new InventorySlot();
        InventorySlot slotDestination = new InventorySlot();

        slotSource.StoreItem(testItem1, 10);
        slotDestination.StoreItem(testItem2, 10);

        bool succeeded = false;

        try
        {
            slotSource.MoveTo(slotDestination, 4);
        }
        catch
        {
            succeeded = true;
        }

        Assert.IsTrue(succeeded);
    }

    [Test]
    public void MoveAdd()
    {
        InventoryItem testItem = ScriptableObject.CreateInstance<InventoryItem>();

        InventorySlot slotSource = new InventorySlot();
        InventorySlot slotDestination = new InventorySlot();

        slotSource.StoreItem(testItem, 2);
        slotDestination.StoreItem(testItem, 4);

        slotSource.MoveAllTo(slotDestination);

        Assert.AreEqual(slotSource.Item, null);
        Assert.AreEqual(slotSource.Quantity, 0);
        Assert.AreEqual(slotDestination.Item, testItem);
        Assert.AreEqual(slotDestination.Quantity, 6);
    }

    [Test]
    public void MoveWithExchange()
    {
        InventoryItem testItem1 = ScriptableObject.CreateInstance<InventoryItem>();
        InventoryItem testItem2 = ScriptableObject.CreateInstance<InventoryItem>();

        InventorySlot slotSource = new InventorySlot();
        InventorySlot slotDestination = new InventorySlot();

        slotSource.StoreItem(testItem1, 2);
        slotDestination.StoreItem(testItem2, 4);

        slotSource.MoveAllTo(slotDestination);

        Assert.AreEqual(slotSource.Item, testItem2);
        Assert.AreEqual(slotSource.Quantity, 4);
        Assert.AreEqual(slotDestination.Item, testItem1);
        Assert.AreEqual(slotDestination.Quantity, 2);
    }
}