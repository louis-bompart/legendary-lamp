using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Cheat : MonoBehaviour
    {

        private InventoryModel model;
        private InventoryController controller;
        public enum Inventory
        {
            Ship,
            Base
        }
        public Inventory inventoryType;

        // Use this for initialization
        void Start()
        {
            model = FindObjectOfType<InventoryModel>();
            List<InventoryController> controllers = new List<InventoryController>(FindObjectsOfType<InventoryController>());
            InventoryController.Inventory type = InventoryController.Inventory.Both;
            switch (inventoryType)
            {
                case Inventory.Ship:
                    type = InventoryController.Inventory.Ship;
                    break;
                case Inventory.Base:
                    type = InventoryController.Inventory.Base;
                    break;
                default:
                    break;
            }
            controller = controllers.Find(x => x.inventoryType == type);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddItems()
        {
            switch (inventoryType)
            {
                case Inventory.Ship:
                    for (int i = 0; i < 6; i++)
                    {
                        controller.AddItem(model.database.FetchItemByID(100 + i), 5);
                    }
                    break;
                case Inventory.Base:
                    for (int i = 0; i < 6; i++)
                    {
                        controller.AddItem(model.database.FetchItemByID(100 + i), 5);
                    }
                    break;
                default:
                    break;
            }


        }
    }
}