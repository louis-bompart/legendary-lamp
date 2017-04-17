using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemNS;

namespace Inventory
{
    public class InventoryModel : MonoBehaviour
    {
        internal List<Slot> slots;
        private List<Slot> reserved;
        internal Dictionary<Item, int> items;
        private int size;
        internal ItemDatabase database;
        private List<IInventoryObserver> views;
        public enum Inventory
        {
            Ship,
            Base
        }
        public Inventory inventoryType;

        public void Awake()
        {
            ItemDatabase.GetInstance(out database);
            views = new List<IInventoryObserver>();
            items = new Dictionary<Item, int>();
            slots = new List<Slot>();
            reserved = new List<Slot>();
            switch (inventoryType)
            {
                case Inventory.Ship:
                    size = 20;
                    break;
                case Inventory.Base:
                    size = 48;
                    break;
                default:
                    break;
            }
            for (int i = 0; i <= size; i++)
            {
                slots.Add(new Slot(this));
            }
        }

        internal Slot GetFreeSlot()
        {
            Slot toReturn = slots.Find(x => !reserved.Contains(x) && x.IsEmpty());
            reserved.Add(toReturn);
            return toReturn;
        }

        internal void FreeSlotReservation(Slot slot)
        {
            reserved.Remove(slot);
        }

        public int GetNbFreeSlot()
        {
            return slots.FindAll(x => x.IsEmpty() && !reserved.Contains(x)).Count;
        }

        public bool HasFreeSlot()
        {
            return slots.Exists(x => x.IsEmpty() && !reserved.Contains(x));
        }

        internal void UnRegisterView(InventoryView inventoryView)
        {
            views.Remove(inventoryView);
        }

        internal void RegisterView(InventoryView inventoryView)
        {
            views.Add(inventoryView);
        }

        internal void NotifyViews()
        {
            foreach (IInventoryObserver view in views)
            {
                view.OnInventoryChange();
            }
        }
    }
}
