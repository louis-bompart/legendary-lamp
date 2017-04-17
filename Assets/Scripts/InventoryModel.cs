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
        public enum Inventory
        {
            Ship,
            Base
        }
        public Inventory inventoryType;

        public void Awake()
        {
            ItemDatabase.GetInstance(out database);
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
    }
}
