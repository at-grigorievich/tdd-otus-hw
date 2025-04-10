using System;
using System.Collections.Generic;

namespace ATG.Items.Inventory
{
    [Serializable]
    public class Inventory
    {
        public readonly List<Item> Items = new();
        
        public event Action<Item> OnItemAdded;
        public event Action<Item> OnItemRemoved;
        public event Action<Item> OnItemConsumed;
        public event Action<Item> OnItemAddStacked; 
        public event Action<Item> OnItemRemoveStacked; 
        
        public void NotifyItemAdded(Item item)
        {
            OnItemAdded?.Invoke(item);
        }

        public void NotifyItemRemoved(Item item)
        {
            OnItemRemoved?.Invoke(item);
        }

        public void NotifyItemConsumed(Item item)
        {
            OnItemConsumed?.Invoke(item);
        }

        public void NotifyItemAddStacked(Item item)
        {
            OnItemAddStacked?.Invoke(item);
        }
        
        public void NotifyItemRemoveStacked(Item item)
        {
            OnItemRemoveStacked?.Invoke(item);
        }
    }
}