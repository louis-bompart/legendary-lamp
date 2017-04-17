using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using ItemNS;
public class Initializer : MonoBehaviour {
    public GameObject slotView;
    public GameObject itemView;
    public TextAsset items;
    public static bool used = false;
    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (!used)
        {
            ItemDatabase.Initalize(items);
            used = true;
            SlotView.prefab = slotView;
            ItemView.prefab = itemView;
        }
    }
}
