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
        public Slot(InventoryModel model, Item item = null, int amount = 0)
        {
            this.model = model;
            this.item = item;
            this.amount = amount;
        }

        public bool HasItem(Item item)
        {
            if (item == null || this.item == null)
                return false;
            return item.Equals(this.item);
        }

        public int RemainingSpace()
        {
            return amount - item.stackSize;
        }

        public bool IsEmpty()
        {
            return item == null && amount == 0;
        }
        public void AddItem(Item item, int amount)
        {
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
                this.item = item;
                this.amount += amount;
                if (model.items.ContainsKey(item))
                    model.items[item] += amount;
                else
                    model.items.Add(item, amount);
                model.FreeSlotReservation(this);
            }
            else
                Debug.LogError("Trying to add a different kind of item in a slot");
        }

        internal void Swap(Slot other)
        {
            if (other.item == this.item)
            {
                int tranferableAmount = Mathf.Min(other.amount, RemainingSpace());
                other.amount -= tranferableAmount;
                this.amount += tranferableAmount;
                if (other.amount == 0)
                {
                    other.item = null;
                }
                return;
            }
            Item item = this.item;
            int amount = this.amount;
            this.amount = other.amount;
            this.item = other.item;
            other.item = item;
            other.amount = amount;
        }

        public void ClearSlot()
        {
            this.amount = 0;
            this.item = null;
        }
        public void RemoveItem(int amount)
        {
            if (amount > this.amount)
            {
                Debug.LogError("Not enough " + item.title + " in this slot");
                return;
            }
            if (amount == this.amount)
                ClearSlot();
            else
                this.amount -= amount;
            model.items[item] -= amount;
        }
    }
}