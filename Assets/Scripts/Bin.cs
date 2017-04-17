using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    internal class Bin : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            ItemView dragged = eventData.pointerDrag.GetComponent<ItemView>();
            SlotView other = dragged.currentSlot;
            other.data.ClearSlot();
            other.inventoryView.LoadInventory();
        }
    }
}