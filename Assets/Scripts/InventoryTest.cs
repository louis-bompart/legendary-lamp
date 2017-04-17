using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
    public class InventoryTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            if (FindObjectsOfType<InventoryTest>().Length > 1)
            {
                DestroyImmediate(this);
                return;
            }
            DontDestroyOnLoad(gameObject);

            AddItems();
        }

        private void AddItems()
        {
            InventoryModel model = FindObjectOfType<InventoryModel>();
            InventoryController[] controller = FindObjectsOfType<InventoryController>();
            controller[1].AddItem(model.database.FetchItemByID(100), 5);
            controller[0].AddItem(model.database.FetchItemByID(100), 5);
            controller[0].AddItem(model.database.FetchItemByID(101), 5);
            controller[1].AddItem(model.database.FetchItemByID(102), 5);
            controller[0].AddItem(model.database.FetchItemByID(103), 5);
            controller[1].AddItem(model.database.FetchItemByID(104), 5);
            controller[0].AddItem(model.database.FetchItemByID(105), 5);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene("Blabla");
            if (Input.GetKeyDown(KeyCode.A))
                AddItems();
        }
    }
}