using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemNS;

namespace Inventory
{
    public class Tooltip : MonoBehaviour
    {
        /// <summary>
        /// The item the tooltip is linked to.
        /// </summary>
        private Item item;
        public static Tooltip instance;
        /// <summary>
        /// The text of the tooltip.
        /// </summary>
        private string data;
        public Color textColor;

        void Awake()
        {
            screenRect = Camera.main.pixelRect;
            if (instance != null)
                Destroy(instance);
            instance = this;
            gameObject.SetActive(false);
        }
        Rect screenRect;
        public Vector3 position;
        public Vector2 dimension;
        /// <summary>
        /// the gameObject follows the mouse Position.
        /// </summary>
        void Update()
        {
            Vector2 pivot = Vector2.zero;
            position = Input.mousePosition;
            Rect rect = GetComponent<RectTransform>().rect;
            dimension = rect.size;
            if (Input.mousePosition.x + rect.width > screenRect.max.x * .95f)
            {
                pivot.x = 1;
            }
            if (Input.mousePosition.y + rect.height > screenRect.max.y*.95f)
            {
                pivot.y = 1;
            }
            GetComponent<RectTransform>().pivot = pivot;
            gameObject.transform.position = position;
        }

        /// <summary>
        /// Activate the gameObject of the specified item
        /// </summary>
        /// <param name="item">Item.</param>
        public void Activate(Item item)
        {
            this.item = item;
            ConstructDataString();
            Update();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivate the gameObject
        /// </summary>
        public void Desactivate()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Constructs the data string according to the item.
        /// </summary>
        public void ConstructDataString()
        {
            data = "<color=#" + ColorUtility.ToHtmlStringRGB(textColor) + "><b>" + item.title + "</b></color>\n" + item.description;
            gameObject.transform.GetChild(0).GetComponent<Text>().text = data;
            LayoutRebuilder.MarkLayoutForRebuild(this.transform as RectTransform);
        }
    }
}