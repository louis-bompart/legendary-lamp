using UnityEngine;

namespace ItemNS
{
    [System.Serializable]
    public class Item
    {
        internal Sprite Sprite { get; private set; }

        public int id;
        public string title;
        public int price;
        public string description;
        public int stackSize;
        public string slug;
        internal void SetSprite()
        {
            this.Sprite = Resources.Load<Sprite>("UIImage/" + slug);
        }

        public Item(int id, string title, int value, string description, int stackSize, string slug)
        {
            this.id = id;
            this.title = title;
            this.price = value;
            this.description = description;
            this.stackSize = stackSize;
            this.slug = slug;
            SetSprite();
        }

        public Item()
        {
            this.id = -1;
        }

        public override bool Equals(object obj)
        {
            return (obj as Item).id == this.id;
        }
        public override int GetHashCode()
        {
            return id;
        }
    }
}