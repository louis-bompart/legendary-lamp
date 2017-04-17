using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemNS;

namespace Inventory
{
    public class Slot
    {
        public Item item;
        public int amount;
        public InventoryModel model;
        private static IDManager idManager;
        internal int id;

        public Slot(InventoryModel model, Item item = null, int amount = 0)
        {
            if (idManager == null)
                idManager = new IDManager();
            this.id = idManager.GetNewID();
            this.model = model;
            this.item = item;
            this.amount = amount;
        }

        ~Slot()
        {
            idManager.FreeID(id);
        }

        internal bool HasItem(Item item)
        {
            if (item == null || this.item == null)
                return false;
            return item.Equals(this.item);
        }

        internal int RemainingSpace()
        {
            return item.stackSize - amount;
        }

        internal bool IsEmpty()
        {
            return item == null && amount == 0;
        }
        internal void AddItem(Item item, int amount)
        {
            model.FreeSlotReservation(this);
            if (item == null)
            {
                Debug.LogError("Cannot add null as an item");
                return;
            }
            bool canDo = (this.item == null);
            if (!canDo)
                canDo = item.Equals(this.item);
            if (canDo)
            {
                int posThis = this.model.slots.IndexOf(this);
                model.slots.Remove(this);
                this.item = item;
                this.amount += amount;
                if (model.items.ContainsKey(item))
                    model.items[item] += amount;
                else
                    model.items.Add(item, amount);
                this.model.slots.Insert(posThis, this);
                model.NotifyViews();
            }
            else
                Debug.LogError("Trying to add a different kind of item in a slot");
        }

        internal void Swap(Slot other)
        {
            int posThis = this.model.slots.IndexOf(this);
            int posOther = other.model.slots.IndexOf(other);
            this.model.slots.Remove(this);

            other.model.slots.Remove(other);
            if (other.item == this.item)
            {
                int tranferableAmount = Mathf.Min(other.amount, RemainingSpace());
                other.amount -= tranferableAmount;
                this.amount += tranferableAmount;
                if (other.amount == 0)
                {
                    other.item = null;
                }
            }
            else
            {
                Item item = this.item;
                int amount = this.amount;
                this.amount = other.amount;
                this.item = other.item;
                other.item = item;
                other.amount = amount;
            }
            this.model.slots.Insert(posThis, this);
            other.model.slots.Insert(posOther, other);
            this.model.NotifyViews();
            other.model.NotifyViews();
        }

        internal void ClearSlot()
        {
            int posThis = this.model.slots.IndexOf(this);
            this.model.slots.Remove(this);
            this.amount = 0;
            this.item = null;
            this.model.slots.Insert(posThis, this);
            model.NotifyViews();

        }
        internal void RemoveItem(int amount)
        {
            if (amount > this.amount)
            {
                Debug.LogError("Not enough " + item.title + " in this slot");
                return;
            }
            int posThis = this.model.slots.IndexOf(this);
            this.model.slots.Remove(this);
            if (amount == this.amount)
                ClearSlot();
            else
            {
                this.amount -= amount;
                model.NotifyViews();
            }
            model.items[item] -= amount;
            this.model.slots.Insert(posThis, this);
            this.model.NotifyViews();
        }
    }
}