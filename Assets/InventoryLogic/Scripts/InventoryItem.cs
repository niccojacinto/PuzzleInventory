using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem {

    public List<Vector2> itemSlots;
    public InventoryItem(SlotShape slotShape) {
        itemSlots = SlotShape.FilterPoints(slotShape);
    }

    public void Rotate(Vector2 pivotPoint, int direction) {
        if (Mathf.Abs(direction) != 1) return;
        List<Vector2> newPoints = new List<Vector2>();

        // Rotate 90 degrees CCW (dir = 1) or CW ( dir = -1 )
        Matrix4x4 rotationMatrix = new Matrix4x4(
            new Vector4(0, 1),
            new Vector4(direction, 0),
            new Vector4(0, 0, 1),
            new Vector4(0, 0, 0, 1)
        );
        foreach (Vector2 v2 in itemSlots)
        {
            // Vector2 relativeVector = v2 - pivotPoint;
            // Vector2 translationVector = rotationMatrix.MultiplyPoint3x4(relativeVector);
            // Vector2 absoluteVector = pivotPoint + translationVector;

            // Simplified...          
            newPoints.Add((Vector2)rotationMatrix.MultiplyPoint3x4(v2-pivotPoint)+pivotPoint);
        }
        itemSlots = newPoints;
    }

    public override string ToString() {
        string s = "";

        foreach (Vector2 v2 in itemSlots)
        {
            s += v2.ToString() + "\n";
        }
        return s;
    }

}
