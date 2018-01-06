using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour {

    [SerializeField]
    GameObject _inventorySlotPrefab;
    InventorySlotUI[,] _inventorySlots;
    Vector2 offset = Vector2.zero;
    Vector2 _dimensions;
    float slotSize;

    private void Start() {
        SlotShape ss = new SlotShape(new int[,]
            {
                {1,0,1,1,1,1,1},
                {1,0,1,1,1,1,1},
                {0,0,0,1,1,1,0},
                {1,0,0,0,0,0,0},
                {1,1,1,1,1,1,1}
            });
        Init(ss);

    }
    

    // Only Use Construct For Tests
    public void Construct(GameObject inventorySlotPrefab) {
        _inventorySlotPrefab = inventorySlotPrefab;
    }

    public void Init(SlotShape slotShape) {
        _inventorySlots =
            new InventorySlotUI[slotShape.Shape.GetLength(0), slotShape.Shape.GetLength(1)];
        slotSize = _inventorySlotPrefab.GetComponent<RectTransform>().sizeDelta.x;
        if (transform.GetComponent<RectTransform>() != null)
        {
            _dimensions = new Vector2(slotShape.Shape.GetLength(1) * slotSize, slotShape.Shape.GetLength(0) * slotSize);
            transform.GetComponent<RectTransform>().sizeDelta = _dimensions;
        }
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
    bool CreateInventorySlot(int x, int y) {
        if (GetSlotAt(x, y) == null)
        {
            _inventorySlots[y, x] = Instantiate(_inventorySlotPrefab).GetComponent<InventorySlotUI>();
            _inventorySlots[y, x].gameObject.transform.SetParent(transform);

            if (transform.GetComponent<RectTransform>() != null)
            {
                transform.GetComponent<RectTransform>().sizeDelta = _dimensions;
                // Gets the offset to align corners for mouse checking and a square dynamic backgrounf panel..
                offset = new Vector2(transform.GetComponent<RectTransform>().rect.width / 2 - slotSize/2,
                        -transform.GetComponent<RectTransform>().rect.height / 2 + slotSize/2);
            }
            _inventorySlots[y, x].gameObject.transform.localPosition = new Vector2(x, -y) * slotSize - offset;
            _inventorySlots[y, x].SetSlot(x, y);
            return true;
        }
        return false;
    }

    // Returns the InventorySlot at the following inventory grid position
    public InventorySlotUI GetSlotAt(int x, int y) {
        try
        {
            return _inventorySlots[y, x];
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogWarning(e + " ---> Handled Exception.. Returning Null");
            return null;
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }

    // Check the inventory space if the following slot position is active and free
    public bool IsSlotFree(Vector2 itemSlot) {
        InventorySlotUI slot = GetSlotAt((int)itemSlot.x, (int)itemSlot.y);
        if (slot == null || !slot.free)
        {
            // Debug.Log("Slot: " + itemSlot + " is not free" );
            return false;
        }
        return true;
    }

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
    public bool AddItemAtSlot(InventoryItemUI item, int x, int y) {
        // Debug.Log(PrintGrid());
        Debug.Log("Base State:");
        // Debug.Log(PrintLayout());
        Debug.Log("...........");

        foreach (Vector2 slotPosition in item.itemSlots)
        {
            if (!(IsSlotFree(new Vector2(slotPosition.x + x, slotPosition.y + y))))
                return false;
        }

        foreach (Vector2 slotPosition in item.itemSlots)
        {
            GetSlotAt(x + (int)slotPosition.x, y + (int)slotPosition.y).Take();
        }

        Debug.Log("After Adding:");
        // Debug.Log(PrintLayout());

        item.transform.SetParent(transform.parent);
        item.transform.localPosition 
            = GetSlotAt(x, y).transform.localPosition;

        return true;
    }

    public Vector2 GetSlotPositionFromMousePosition(Vector2 mousePosition) {
        Vector2 position = transform.InverseTransformPoint(Input.mousePosition) ;
        int x = Mathf.RoundToInt(position.x / slotSize) + Mathf.RoundToInt(offset.x / slotSize);
        int y = -(Mathf.RoundToInt(position.y / slotSize)) + (-Mathf.RoundToInt(offset.y / slotSize));
        return new Vector2(x,y);
    }

    /* Unhandled for nulls.. only use if necessary
    public string PrintGrid() {
        string line = "";

        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                line += _inventorySlots[y, x] != null ? "[" + x + "," + y + "]" : "   ";

            }
            line += "\n";
        }
        return line;
    }
    */

    public string PrintLayout() {
        string line = "";

        for (int y = 0; y < _inventorySlots.GetLength(0); y++)
        {
            for (int x = 0; x < _inventorySlots.GetLength(1); x++)
            {
                // line += _inventorySlots[y, x] != null ? "[ " + "]" : "   ";

                InventorySlotUI slot = GetSlotAt(x, y);
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
