using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    InventoryManager inventoryManager;

    [SerializeField]
    InventoryItemUI item;

	void Start () {
	    // inventoryManager.AddItemAtSlot(item,2,0);
	}

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 inventoryPosition = inventoryManager.GetSlotPositionFromMousePosition(Input.mousePosition);
            inventoryManager.AddItemAtSlot(item, (int)inventoryPosition.x, (int)inventoryPosition.y);
        }
    }

}
