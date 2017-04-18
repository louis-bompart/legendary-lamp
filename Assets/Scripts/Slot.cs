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
        private static Dictionary<int, Slot> mappedSlots;

        internal static Slot GetSlotFromId(int id)
        {
            Slot toReturn = null;
            mappedSlots.TryGetValue(id, out toReturn);
            return toReturn;
        }
        private static Slot GetSlotFromSlot(Slot slot)
        {
            return GetSlotFromId(slot.id);
        }

        public Slot(InventoryModel model, Item item = null, int amount = 0)
        {
            if (idManager == null)
                idManager = new IDManager();
            this.id = idManager.GetNewID();
            this.model = model;
            this.item = item;
            this.amount = amount;
            if (mappedSlots == null)
                mappedSlots = new Dictionary<int, Slot>();
            mappedSlots.Add(id, this);
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
                GetSlotFromSlot(this).item = item;
                GetSlotFromSlot(this).amount += amount;
                if (model.items.ContainsKey(item))
                    model.items[item] += amount;
                else
                    model.items.Add(item, amount);
                model.NotifyViews();
            }
            else
                Debug.LogError("Trying to add a different kind of item in a slot");
        }

        internal void Swap(Slot other)
        {
            if (other == this)
                return;

            if (other.item == this.item)
            {
                int tranferableAmount = Mathf.Min(other.amount, RemainingSpace());
                GetSlotFromSlot(other).amount -= tranferableAmount;
                GetSlotFromSlot(this).amount += tranferableAmount;
                if (other.amount == 0)
                {
                    GetSlotFromSlot(other).item = null;
                }
            }
            else
            {
                Item item = this.item;
                int amount = this.amount;
                GetSlotFromSlot(this).amount = other.amount;
                GetSlotFromSlot(this).item = other.item;
                GetSlotFromSlot(other).item = item;
                GetSlotFromSlot(other).amount = amount;
            }
            this.model.NotifyViews();
            other.model.NotifyViews();
        }

        internal void ClearSlot()
        {
            GetSlotFromSlot(this).amount = 0;
            GetSlotFromSlot(this).item = null;
            model.NotifyViews();

        }
        internal void RemoveItem(int amount)
        {
            if (amount > this.amount)
            {
                Debug.LogError("Not enough " + item.title + " in this slot");
                return;
            }
            if (amount == this.amount)
                ClearSlot();
            else
            {
                GetSlotFromSlot(this).amount -= amount;
                model.NotifyViews();
            }
            model.items[item] -= amount;
            GetSlotFromSlot(this).model.NotifyViews();
        }
        public override bool Equals(object obj)
        {
            return (obj as Slot).id == this.id;
        }
    }
}