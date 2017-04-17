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
            if (instance != null)
                Destroy(instance);
            instance = this;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// the gameObject follows the mouse Position.
        /// </summary>
        void Update()
        {
            gameObject.transform.position = Input.mousePosition;
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
            data = "<color=#" + textColor.r.ToString() + textColor.g.ToString() + textColor.b.ToString() + textColor.a.ToString() + "><b>" + item.title + "</b></color>\n" + item.description;
            gameObject.transform.GetChild(0).GetComponent<Text>().text = data;
        }
    }
}