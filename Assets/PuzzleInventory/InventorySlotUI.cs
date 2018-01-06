using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour {
	public bool free = true;

    public void Free() {
        free = true;
        GetComponent<Image>().color = Color.white;
    }

    public void Take() {
        free = false;
        GetComponent<Image>().color = Color.red;
    }

    public void SetSlot(int x, int y) {
        // gameObject.GetComponentInChildren<Text>().text = x + "," + y;
    }
}
