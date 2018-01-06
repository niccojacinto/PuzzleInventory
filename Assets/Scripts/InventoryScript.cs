using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {

    GameObject inventorySlotPrefab;

	void Start () {
        SlotShape ss = new SlotShape(
            new int[,]
            {
                {1,1,0,1},
                {0,1,1,1},
                {0,0,1,0}
            }
        );

		Inventory i = new Inventory(ss);   
	}

    void CreateSlots (Inventory inventory) {
        // InventorySlot slot = Instantiate(inventorySlotPrefab);

    }
	
}
