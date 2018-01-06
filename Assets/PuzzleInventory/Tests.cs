using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Tests {

    [Test]
    public void SlotShapeTests() {
        SlotShape ss = new SlotShape();
        Assert.AreEqual("[ ]\n", ss.ToString());

        ss = new SlotShape(new int[,]
            {
                {1, 0, 1}
            });
        Assert.AreEqual("[ ]   [ ]\n", ss.ToString());

        ss = new SlotShape(new int[,]
            {
                {1},
                {0},
                {1}
            });
        Assert.AreEqual("[ ]\n   \n[ ]\n", ss.ToString());

        ss = new SlotShape(new int[,]
            {
                {1,1,1},
                {1,1,1},
                {1,1,1}
            });
        Assert.AreEqual("[ ][ ][ ]\n[ ][ ][ ]\n[ ][ ][ ]\n", ss.ToString());

        ss = new SlotShape(new int[,]
            {
                {0,0,0},
                {0,0,0},
                {0,0,0}
            });
        Assert.AreEqual("         \n         \n         \n", ss.ToString());

        ss = new SlotShape(new int[,]
            {
                {1,0,1},
                {0,1,0},
                {1,0,1}
            });
        Assert.AreEqual(true, ss.IsOpenSlot(0, 0)); Assert.AreEqual(false, ss.IsOpenSlot(1, 0)); Assert.AreEqual(true, ss.IsOpenSlot(2, 0));
        Assert.AreEqual(false, ss.IsOpenSlot(0, 1)); Assert.AreEqual(true, ss.IsOpenSlot(1, 1)); Assert.AreEqual(false, ss.IsOpenSlot(2, 1));
        Assert.AreEqual(true, ss.IsOpenSlot(0, 2)); Assert.AreEqual(false, ss.IsOpenSlot(1, 2)); Assert.AreEqual(true, ss.IsOpenSlot(2, 2));

    }

    [Test]
	public void CreatingInventoryManagerTests() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1, 0, 1},
                {0, 1, 0},
                {1, 0, 1}
            });

        var inv = new GameObject().AddComponent<InventoryManager>();
        var inventorySlotPrefab = (GameObject)Resources.Load("Tests/InventorySlot");
        inv.Construct(inventorySlotPrefab);
        inv.GetComponent<InventoryManager>().Init(ss);

        Assert.AreEqual(ss.ToString(), inv.ToString(), inv.ToString());

        Assert.AreNotEqual(null, inv.GetSlotAt(0,0)); Assert.AreEqual(null, inv.GetSlotAt(1, 0)); Assert.AreNotEqual(null, inv.GetSlotAt(2,0));
        Assert.AreEqual(null, inv.GetSlotAt(0, 1)); Assert.AreNotEqual(null, inv.GetSlotAt(1, 1)); Assert.AreEqual(null, inv.GetSlotAt(2, 1));
        Assert.AreNotEqual(null, inv.GetSlotAt(0, 2)); Assert.AreEqual(null, inv.GetSlotAt(1, 2)); Assert.AreNotEqual(null, inv.GetSlotAt(2, 2));
	}

    [Test]
    public void CreatingInventoryItemUI() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1},
                {1}
            });
        var i = new GameObject().AddComponent<InventoryItemUI>();
        i.GetComponent<InventoryItemUI>().Init(ss);

        Assert.AreEqual(true, i.itemSlots.Contains(new Vector2(0, 0)));
        Assert.AreEqual(true, i.itemSlots.Contains(new Vector2(0, 1)));

        ss = new SlotShape(new int[,]
            {
                {0, 1},
                {1, 0}
            });
        i = new GameObject().AddComponent<InventoryItemUI>();
        i.GetComponent<InventoryItemUI>().Init(ss);

        Assert.AreEqual(2, i.itemSlots.Count);
        Assert.AreEqual(false, i.itemSlots.Contains(new Vector2(0, 0)));
        Assert.AreEqual(true, i.itemSlots.Contains(new Vector2(1, 0)));
        Assert.AreEqual(true, i.itemSlots.Contains(new Vector2(0, 1)));
        Assert.AreEqual(false, i.itemSlots.Contains(new Vector2(1, 1)));
    }

    [Test]
    public void InventorySpaceCheck() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1,1,1,1,1},
                {1,1,1,1,1},
                {1,1,1,1,1}
            });

        var inv = new GameObject().AddComponent<InventoryManager>();
        var inventorySlotPrefab = (GameObject)Resources.Load("Tests/InventorySlot");
        inv.Construct(inventorySlotPrefab);
        inv.GetComponent<InventoryManager>().Init(ss);


        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                // Everything should be free
                Assert.AreEqual(true, inv.IsSlotFree(new Vector2(x,y)), inv.GetSlotAt(x,y).ToString());
            }
        }

        ss = new SlotShape(new int[,]
             {
                {1,1,1,1,1},
                {1,1,0,1,1},
                {1,1,1,1,1}
             });

        inv = new GameObject().AddComponent<InventoryManager>();
        inv.Construct(inventorySlotPrefab);
        inv.GetComponent<InventoryManager>().Init(ss);

        // One space is unavailable therefore it should be null and return false
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(2, 1)));
    }

    [Test]
    public void InventorySpaceAfterAddingItem() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1,1,1,1,1},
                {1,1,0,1,1},
                {1,1,1,1,1}
            });

        var inv = new GameObject().AddComponent<InventoryManager>();
        var inventorySlotPrefab = (GameObject)Resources.Load("Tests/InventorySlot");
        inv.Construct(inventorySlotPrefab);
        inv.GetComponent<InventoryManager>().Init(ss);

        ss = new SlotShape(new int[,]
            {
                {1,1},
                {1,1}
            });

        var ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);

        // Attempt #1 = Add Success
        // Before Adding Item
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(0, 0)), "A1 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(1, 0)), "A1 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(0, 1)), "A1 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(1, 1)), "A1 Before Adding...");

        // Adding Item
        Assert.AreEqual(true, inv.AddItemAtSlot(ii, 0, 0), "A1 Adding...");

        // After Adding Item
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(0, 0)), "A1 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 0)), "A1 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(0, 1)), "A1 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 1)), "A1 After Adding...");

        // Attempt #2 = Add Fail
        // Before Adding Item
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 0)), "A2 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(2, 0)), "A2 Before Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 1)), "A2 Before Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(2, 1)), "A2 Before Adding...");

        // Adding Item
        Assert.AreEqual(false, inv.AddItemAtSlot(ii, 1, 0), "A2 Adding...");

        // After Adding Item
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 0)), "A2 After Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(2, 0)), "A2 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(1, 1)), "A2 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(2, 1)), "A2 After Adding...");

        // Attempt #3 = Add At Different Location + Success
        // Before Adding Item
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(3, 0)), "A3 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(4, 0)), "A3 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(3, 1)), "A3 Before Adding...");
        Assert.AreEqual(true, inv.IsSlotFree(new Vector2(4, 1)), "A3 Before Adding...");

        // Adding Item
        Assert.AreEqual(true, inv.AddItemAtSlot(ii, 3, 0), "A3 Adding...");

        // After Adding Item
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(3, 0)), "A3 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(4, 0)), "A3 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(3, 1)), "A3 After Adding...");
        Assert.AreEqual(false, inv.IsSlotFree(new Vector2(4, 1)), "A3 After Adding...");

        // Attempt #4 = Add Index Out Of Range
        Assert.AreEqual(false, inv.AddItemAtSlot(ii, 3, 2), "A4 Adding...");

        // Attempt #5 = Adding More...
        ss = new SlotShape(new int[,]
            {
                {1,1}
            });
        ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);
        Assert.AreEqual(true, inv.AddItemAtSlot(ii, 1, 2), "A5 Adding...");

        // Attempt #6 = Irregular Shape..
        // Reset Inventory
        ss = new SlotShape(new int[,]
            {
                {1,1,1,1,1},
                {1,1,0,1,1},
                {1,1,1,1,1}
            });

        inv = new GameObject().AddComponent<InventoryManager>();
        inv.Construct(inventorySlotPrefab);
        inv.GetComponent<InventoryManager>().Init(ss);

        ss = new SlotShape(new int[,]
            {
                {1,1,0,0,0},
                {1,1,0,0,0},
                {1,1,1,1,0}
            });
        ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);
        Assert.AreEqual(false, inv.AddItemAtSlot(ii, 1, 0), "A6 Adding...");
        Assert.AreEqual(true, inv.AddItemAtSlot(ii, 0, 0), "A6 Adding...");
    }

    [Test]
    public void InventoryItemRotation() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1}, // (0,0)
                {1}, // (0,1)
                {1} // (0,2)
            });


        var ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);

        // Rotate Pivot Origin (0,0) -> (0,0)(1,0)(2,0)
        ii.Rotate(new Vector2(0, 0), 1);
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(0, 0)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(1, 0)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(2, 0)), ii.ToString());
        // Rotate Pivot (0,1) -> (-1,1)(0,1)(1,1)
        ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);
        ii.Rotate(new Vector2(0, 1), -1);
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(-1, 1)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(0, 1)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(1, 1)), ii.ToString());
        // Rotate Pivot (0,2) -> (-2, 2)(-1, 2)(0,2)
        ii = new GameObject().AddComponent<InventoryItemUI>();
        ii.GetComponent<InventoryItemUI>().Init(ss);
        ii.Rotate(new Vector2(0, 2), 1);
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(-2, 2)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(-1, 2)), ii.ToString());
        Assert.AreEqual(true, ii.itemSlots.Contains(new Vector2(0, 2)), ii.ToString());
    }


    [TearDown]
    public void AfterEveryTest() {
        var ims = GameObject.FindObjectsOfType<InventoryManager>();
        var iss = GameObject.FindObjectsOfType<InventorySlotUI>();
        var iis = GameObject.FindObjectsOfType<InventoryItemUI>();

        foreach (InventoryManager im in ims) GameObject.Destroy(im.gameObject);
        foreach (InventorySlotUI isui in iss) GameObject.Destroy(isui.gameObject);
        foreach (InventoryItemUI ii in iis) GameObject.Destroy(ii.gameObject);
    }
}
