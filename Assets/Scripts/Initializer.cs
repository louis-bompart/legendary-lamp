using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
public class Initializer : MonoBehaviour {
    public GameObject slotView;
    public GameObject itemView;
    private static bool used = false;
    private void Awake()
    {
        if(!used)
        {
            used = true;
            SlotView.prefab = slotView;
            ItemView.prefab = itemView;
        }
    }
}
