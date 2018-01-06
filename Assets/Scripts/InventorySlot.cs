using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotShape {
    private int[,] _shape;
    public int[,] Shape {get {return _shape; }}
    public SlotShape (int[,] slotShape = null) {
        if (slotShape == null)
        {
            _shape = new int[,] { 
                { 1 }
            };
        } else
        {
            _shape = slotShape;
        }
    }

    public bool IsOpenSlot(int xPosition, int yPosition) {
        return _shape[yPosition, xPosition] > 0 ? true : false;   
    }

    // Gets necessary points from the slot shape to only include relevant points
    // ie) in the case of { 0, 0, 1} only  point (2, 0) is relevant 'cause the first two are empty
    // ---------------------------^ this point
    public static List<Vector2> FilterPoints(SlotShape slotShape) {
        List<Vector2> gridSlots = new List<Vector2>();
        for (int y = 0; y < slotShape.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < slotShape.Shape.GetLength(1); x++)
            {
                if (slotShape.IsOpenSlot(x, y)) gridSlots.Add(new Vector2(x, y));
            }
        }
        return gridSlots;
    }

    public override string ToString() {
        string line="";

        // Note: Dimensional Positions are switched in 2D Array.. 
        // X = 1;
        // Y = 0;
        for (int y = 0; y < _shape.GetLength(0); y++)
        {
            for (int x = 0; x < _shape.GetLength(1); x++)
            {
                line += _shape[y, x] > 0 ? "[ " + "]" : "   ";
                // line += string.Format("({0},{1})",x,y);
            }
            line += "\n";
        }
        return line;
    }
}

public class InventorySlot {
    public bool free = true;
}
