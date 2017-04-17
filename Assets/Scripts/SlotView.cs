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
        public Slot data;
        public ItemView itemView;
        public InventoryView inventoryView;

        public static SlotView Create(Slot data, GameObject parent, InventoryView inventoryView)
        {
            GameObject toReturnGO = Instantiate(prefab);
            toReturnGO.transform.SetParent(parent.transform, false);
            SlotView toReturn = toReturnGO.GetComponent<SlotView>();
            toReturn.data = data;
            toReturn.inventoryView = inventoryView;
            return toReturn;
        }
        private void Start()
        {
            UpdateView();
        }
        public void UpdateView()
        {
            data = data.model.slots.Find(x => data.id == x.id);
            if (itemView != null)
                Destroy(itemView);
            if (!data.IsEmpty())
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
            other.data.Swap(data);
        }
    }
}