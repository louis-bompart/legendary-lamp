using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemNS;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private List<InventoryModel> models;
        public enum Inventory
        {
            Ship,
            Base,
            Both
        }
        public Inventory inventoryType;
        private ItemDatabase database;

        /// <summary>
        /// Add a certain amount of an item from the attached inventories if possible, does nothing otherwise.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to add</param>
        /// <returns>True if the operation is a success, false otherwise and if their is no item with this id.</returns>
        public bool AddItem(int id, int amount)
        {
            Item item = database.FetchItemByID(id);
            if (item != null)
                return AddItem(item, amount);
            Debug.LogError("Item with " + id + " doesn't exist !");
            return false;

        }

        /// <summary>
        /// Add a certain amount of an item from the attached inventories if possible, does nothing otherwise.
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The amount to add</param>
        /// <returns>True if the operation is a success, false otherwise.</returns>
        public bool AddItem(Item item, int amount)
        {
            List<Slot> slotToUse = new List<Slot>();
            if (CanItFit(item, amount, ref slotToUse))
            {
                foreach (Slot slot in slotToUse)
                {
                    if (amount > 0)
                    {
                        int qteitem = Mathf.Min(amount, slot.IsEmpty() ? item.stackSize : item.stackSize - slot.amount);
                        slot.AddItem(item, qteitem);
                        amount -= qteitem;
                    }
                    slot.model.FreeSlotReservation(slot);
                }
                return true;
            }
            else
            {
                Debug.Log("Not enough space for this item !");
                return false;
            }
        }

        /// <summary>
        /// Remove a certain amount of an item from the attached inventories if possible, does nothing otherwise.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <param name="amount">The amount to remove</param>
        /// <returns>True if the operation is a success, false otherwise and if their is no item with this id.</returns>
        public bool RemoveItem(int id, int amount)
        {
            Item item = database.FetchItemByID(id);
            if (item != null)
                return RemoveItem(item, amount);
            Debug.LogError("Item with " + id + " doesn't exist !");
            return false;
        }

        /// <summary>
        /// Remove a certain amount of an item from the attached inventories if possible, does nothing otherwise.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <param name="amount">The amount to remove</param>
        /// <returns>True if the operation is a success, false otherwise.</returns>
        public bool RemoveItem(Item item, int amount)
        {
            int amountAvailable = 0;
            foreach (InventoryModel model in models)
            {
                int val = 0;
                if (model.items.TryGetValue(item, out val))
                    amountAvailable += val;
            }
            if (amountAvailable < amount)
                return false;
            List<Slot> slotsToUse = new List<Slot>();
            foreach (InventoryModel model in models)
            {
                slotsToUse.AddRange(model.slots.FindAll(x => x.HasItem(item)));
            }
            foreach (Slot slot in slotsToUse)
            {
                if (amount <= 0)
                    break;
                int qteToRemove = Mathf.Min(amount, slot.amount);
                amount -= qteToRemove;
                slot.RemoveItem(qteToRemove);
            }
            return true;
        }
        /// <summary>
        /// Check if you can fit all those item in the attached inventories
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The quantity of the item to add</param>
        /// <returns>True if you can, false otherwise or if their is no item with this id</returns>
        public bool CanItFit(int id, int amount)
        {
            Item item = database.FetchItemByID(id);
            if (item != null)
                return CanItFit(item, amount);
            Debug.LogError("Item with " + id + " doesn't exist !");
            return false;
        }

        /// <summary>
        /// Check if you can fit all those item in the attached inventories
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="amount">The quantity of the item to add</param>
        /// <returns>True if you can, false otherwise</returns>
        public bool CanItFit(Item item, int amount)
        {
            List<Slot> uselessButNeeded = new List<Slot>();
            return CanItFit(item, amount, ref uselessButNeeded);
        }

        private bool CanItFit(Item item, int amount, ref List<Slot> slotsToUse)
        {
            foreach (InventoryModel model in models)
            {
                slotsToUse.AddRange(model.slots.FindAll(x => x.HasItem(item)));
            }
            int freeSpace = 0;
            foreach (Slot slot in slotsToUse)
            {
                freeSpace += slot.RemainingSpace();
            }
            if (amount > freeSpace)
            {
                int nbSlotRequired = Mathf.CeilToInt((float)(amount - freeSpace) / (float)item.stackSize);
                int nbFreeSlot = 0;
                foreach (InventoryModel model in models)
                {
                    int slotsitem = Mathf.Min(model.GetNbFreeSlot(), nbSlotRequired - nbFreeSlot);
                    nbFreeSlot += slotsitem;
                    for (int i = 0; i < slotsitem; i++)
                    {
                        slotsToUse.Add(model.GetFreeSlot());
                    }
                }
                if (nbFreeSlot < nbSlotRequired)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Return the total quantity of the item in the attached inventories
        /// </summary>
        /// <param name="id">The id of the item searched</param>
        /// <returns>Total quantity of the item, or -1 if their is no item with this id</returns>
        public int GetQuantity(int id)
        {
            Item item = database.FetchItemByID(id);
            if (item != null)
                return GetQuantity(item);
            Debug.LogError("Item with " + id + " doesn't exist !");
            return -1;
        }

        /// <summary>
        /// Return the total quantity of the item in the attached inventories
        /// </summary>
        /// <param name="item">The item searched</param>
        /// <returns>Total quantity of the item</returns>
        public int GetQuantity(Item item)
        {
            int quantity = 0;
            foreach (InventoryModel model in models)
            {
                if (model.items.ContainsKey(item))
                    quantity += model.items[item];
            }
            return quantity;
        }

        private void Awake()
        {
            ItemDatabase.GetInstance(out database);
            models = new List<InventoryModel>();
            List<InventoryModel> allModels = new List<InventoryModel>(FindObjectsOfType<InventoryModel>());
            switch (inventoryType)
            {
                case Inventory.Ship:
                    AddInventoryModel(InventoryModel.Inventory.Ship, allModels);
                    break;
                case Inventory.Base:
                    AddInventoryModel(InventoryModel.Inventory.Base, allModels);
                    break;
                case Inventory.Both:
                    AddInventoryModel(InventoryModel.Inventory.Ship, allModels);
                    AddInventoryModel(InventoryModel.Inventory.Base, allModels);
                    break;
                default:
                    break;
            }

        }

        private void AddInventoryModel(InventoryModel.Inventory invType, List<InventoryModel> allModels)
        {
            models.Add(allModels.Find(model => model.inventoryType == invType));
        }
    }
}