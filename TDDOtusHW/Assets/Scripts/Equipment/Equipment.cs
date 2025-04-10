using System;
using System.Collections.Generic;
using ATG.OtusHW.Inventory;

namespace ATG.Items.Equipment
{
    [Serializable]
    public class Equipment
    {
        public List<Item> ItemsDebug = new(); // only for editor debug
        public readonly Dictionary<EquipType, Item> Items = new();

        public event Action<Item> OnItemTakeOn;
        public event Action<Item> OnItemTakeOff;
        
        public void NotifyItemTakeOn(Item item)
        {
            OnItemTakeOn?.Invoke(item);
        }

        public void NotifyItemTakeOff(Item item)
        {
            OnItemTakeOff?.Invoke(item);
        }
    }
}