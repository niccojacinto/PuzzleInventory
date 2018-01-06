using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory {

    InventorySlot[,] _inventorySlots;

    // Creates an inventory with the following slotShape
    public Inventory(SlotShape slotShape) {
        _inventorySlots =
            new InventorySlot[slotShape.Shape.GetLength(0), slotShape.Shape.GetLength(1)];

        PopulateInventory(slotShape);

    }

    // Populates Inventory with InventorySlots along the possible areas of the slotShape used.
    // Creating A Slot: SlotShape ---   InventorySlot
    //                 { 1 1 1 }      [ ] [ ] [ ]  
    //                 { 1 1 1 } ---> [ ] [ ] [ ]   = 7 created Slots
    //                 { 0 1 0 }      ... [ ] ...
    private int PopulateInventory(SlotShape slotShape) {
        int slotsCreated = 0;
        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                if (slotShape.IsOpenSlot(x, y))
                    if (CreateInventorySlot(x, y)) slotsCreated++;
            }
        }
        return slotsCreated;
    }

    // Creates an InventorySlot at the following inventory grid position
    private bool CreateInventorySlot(int x, int y) {
        if (GetSlotAt(x, y) == null) {
            _inventorySlots[y, x] = new InventorySlot();
            return true;
        }
        return false;
    }

    // Returns the InventorySlot at the following inventory grid position
    public InventorySlot GetSlotAt(int x, int y) {
        try {
            return _inventorySlots[y, x];
        } catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning(e + " ---> Handled Exception.. Returning Null");
            return null;
        }
    }

    public bool IsSlotFree(Vector2 itemSlot) {
        InventorySlot slot = GetSlotAt((int)itemSlot.x, (int)itemSlot.y);
        if (slot == null || !slot.free) {
            // Debug.Log("Slot: " + itemSlot + " is not free" );
            return false;
        } 
        return true;
    }

    // Checks the inventory space if the following slot positions are active and free
    public bool AreSlotsFree(List<Vector2> itemSlots) {
        foreach (Vector2 v2 in itemSlots)
        {
            if (!IsSlotFree(v2)) return false;
        }
        return true;
    }

    // Adds an Item at origin x, y using item's space
    // TODO: Prechecks for performance
    // A: Check if item shape > inventory shape (very unlikely)
    // B: Keep track of remaining space and compare remaining space before anything else
    public bool AddItemAtSlot(InventoryItem item, int x, int y) {
        Debug.Log(PrintGrid());
        Debug.Log("Base State:");
        Debug.Log(PrintLayout());
        Debug.Log("...........");

        foreach (Vector2 slotPosition in item.itemSlots)
        {
            if (!(IsSlotFree(new Vector2(slotPosition.x + x, slotPosition.y + y))))
                return false;
        }

        foreach (Vector2 slotPosition in item.itemSlots)
        {
            GetSlotAt(x+(int)slotPosition.x, y+(int)slotPosition.y).free = false;
        }

        Debug.Log("After Adding:");
        Debug.Log(PrintLayout());

        return true;
    }

    public string PrintGrid() {
        string line = "";

        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                line += _inventorySlots[y, x] != null ? "["+x+","+y + "]" : "   ";

            }
            line += "\n";
        }
        return line;
    }

    public string PrintLayout() {
        string line = "";

        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                // line += _inventorySlots[y, x] != null ? "[ " + "]" : "   ";

                InventorySlot slot = GetSlotAt(x, y);
                if (slot == null) line += "   ";
                else if (slot.free) line += "[ ]";
                else if (!slot.free) line += "[.]";
            }
            line += "\n";
        }
        return line;
    }

    public override string ToString() {
        string line = "";

        // Note: Dimensional Positions are switched in 2D Array.. 
        // X = 1;
        // Y = 0;
        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                line += _inventorySlots[y, x] != null ? "[ " + "]" : "   ";
            }
            line += "\n";
        }
        return line;
    }
}
