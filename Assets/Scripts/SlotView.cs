using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Inventory
{
    public class SlotView : MonoBehaviour, IDropHandler
    {
        public static GameObject prefab;
        public int slotID;
        public ItemView itemView;
        public InventoryView inventoryView;

        public static SlotView Create(int slotID, GameObject parent, InventoryView inventoryView)
        {
            GameObject toReturnGO = Instantiate(prefab);
            toReturnGO.transform.SetParent(parent.transform, false);
            SlotView toReturn = toReturnGO.GetComponent<SlotView>();
            toReturn.slotID = slotID;
            toReturn.inventoryView = inventoryView;
            return toReturn;
        }
        private void Start()
        {
            UpdateView();
        }
        public void UpdateView()
        {
            if (itemView != null)
            {
                Destroy(itemView.gameObject);
                itemView = null;
            }
            if (!Slot.GetSlotFromId(slotID).IsEmpty())
            {
                ItemView itemView = ItemView.Create(this);
                itemView.transform.SetParent(transform, false);
                this.itemView = itemView;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemView dragged = eventData.pointerDrag.GetComponent<ItemView>();
            SlotView other = dragged.currentSlot;
            dragged.currentSlot = this;
            Slot.GetSlotFromId(slotID).Swap(Slot.GetSlotFromId(other.slotID));
        }
    }
}