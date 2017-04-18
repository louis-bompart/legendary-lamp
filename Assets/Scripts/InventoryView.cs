using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryView : MonoBehaviour, IInventoryObserver
    {
        internal InventoryModel model;
        private GridLayoutGroup layout;
        internal List<SlotView> slotViews;
        public enum Inventory
        {
            Ship,
            Base
        }
        public Inventory inventory;
        // Use this for initialization
        void Awake()
        {
            slotViews = new List<SlotView>();
            layout = GetComponent<GridLayoutGroup>();
            //TODO Use Data, Luke.
            List<InventoryModel> allModels = new List<InventoryModel>(FindObjectsOfType<InventoryModel>());
            switch (inventory)
            {
                case Inventory.Ship:
                    SetInventoryModel(InventoryModel.Inventory.Ship, allModels);
                    break;
                case Inventory.Base:
                    SetInventoryModel(InventoryModel.Inventory.Base, allModels);
                    break;
                default:
                    break;
            }
            model.RegisterView(this);
            LoadInventory(true);
        }

        private void SetInventoryModel(InventoryModel.Inventory invType, List<InventoryModel> allModels)
        {
            model = allModels.Find(model => model.inventoryType == invType);
        }

        // Update is called once per frame
        void Update()
        {

        }
        void LoadInventory(bool resize = false)
        {
            if (resize)
                GetComponent<GridLayoutGroup>().cellSize = ComputeCellSize();
            for (int i = 0; i < slotViews.Count; i++)
            {
                Destroy(slotViews[i].gameObject);
            }
            foreach (Slot slot in model.slots)
            {
                slotViews.Add(SlotView.Create(slot.id, gameObject, this));
            }
        }

        private Vector2 ComputeCellSize()
        {
            float width = transform.parent.GetComponent<RectTransform>().rect.width - transform.parent.GetComponent<VerticalLayoutGroup>().padding.horizontal;
            float height = GetComponent<RectTransform>().rect.height - layout.padding.vertical;
            float size = Mathf.Min(width, height) / Mathf.Sqrt(model.slots.Count);
            return new Vector2(size - layout.spacing.x, size - layout.spacing.y);
        }
        private void OnDestroy()
        {
            model.UnRegisterView(this);
        }
        void IInventoryObserver.OnInventoryChange()
        {
            foreach (SlotView slot in slotViews)
            {
                slot.UpdateView();
            }
        }
    }
}
