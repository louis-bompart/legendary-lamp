using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace ItemNS
{
    [System.Serializable]
    public class ItemDatabase
    {
        [SerializeField]
        private List<Item> database = new List<Item>();
        private Dictionary<int, Item> databaseIndexed;
        private static ItemDatabase instance;

        private ItemDatabase(TextAsset itemDataBaseJson)
        {
            instance = JsonUtility.FromJson<ItemDatabase>(itemDataBaseJson.text);
            instance.databaseIndexed = new Dictionary<int, Item>();
            foreach (Item item in instance.database)
            {
                item.SetSprite();
                instance.databaseIndexed.Add(item.id, item);
            }
        }

        public Item FetchItemByID(int id)
        {
            Item item = null;
            databaseIndexed.TryGetValue(id, out item);
            return item;
        }

        public static ItemDatabase Instance(TextAsset json)
        {
            if (instance == null)
            {
                new ItemDatabase(json);
            }
            return instance;
        }
    }
}