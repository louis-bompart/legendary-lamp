using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory { 
public class Cheat : MonoBehaviour {

    private InventoryModel model;
    private InventoryController[] controller;
    public enum Inventory
    {
        Ship,
        Base
    }
    public Inventory inventoryType;

        // Use this for initialization
    void Start () {
        model = FindObjectOfType<InventoryModel>();
        controller = FindObjectsOfType<InventoryController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItems() {
            switch (inventoryType)
            {
                case Inventory.Ship:
                    for (int i = 0; i < 6; i++)
                    {
                        controller[0].AddItem(model.database.FetchItemByID(100+i), 5);
                    }
                    break;
                case Inventory.Base:
                    for (int i = 0; i < 6; i++)
                    {
                        controller[1].AddItem(model.database.FetchItemByID(100 + i), 5);
                    }
                    break;
                default:
                    break;
            }
            

    }
}
}