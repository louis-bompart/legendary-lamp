using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        internal static GameObject prefab;
        internal int amount;
        public Text amountView;
        internal SlotView currentSlot;

        internal static ItemView Create(SlotView slot)
        {
            ItemView toReturn = Instantiate(prefab).GetComponent<ItemView>();
            toReturn.currentSlot = slot;
            toReturn.amount = Slot.GetSlotFromId(slot.slotID).amount;
            toReturn.amountView.text = toReturn.amount.ToString();
            toReturn.GetComponent<Image>().sprite = Slot.GetSlotFromId(slot.slotID).item.Sprite;
            return toReturn;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(currentSlot.transform.root, true);
            this.transform.position = eventData.position;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(currentSlot.transform, true);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.instance.Activate(Slot.GetSlotFromId(currentSlot.slotID).item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.instance.Desactivate();
        }
    }
}