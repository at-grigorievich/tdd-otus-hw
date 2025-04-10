using System.Linq;

namespace ATG.Items.Inventory
{
    public static class InventoryUseCases
    {
        public static void AddItem(Inventory inventory, Item item)
        {
            if (TryAddStackItem(inventory, item) == true) return;

            inventory.Items.Add(item);
            inventory.NotifyItemAdded(item);
        }

        public static void AddItems(Inventory inventory, Item item, int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddItem(inventory, item);
            }
        }

        public static Item RemoveItem(Inventory inventory, Item item, bool removeByRef = false)
        {
            Item res = removeByRef == false
                ? inventory.Items.FirstOrDefault(i => i.Id == item.Id)
                : inventory.Items.FirstOrDefault(i => ReferenceEquals(item, i));

            if (res == null) return null;

            if (TryRemoveStackItem(inventory, res) == true) return res;

            inventory.Items.Remove(res);
            inventory.NotifyItemRemoved(res);

            return res;
        }

        public static Item RemoveItem(Inventory inventory, ItemConfig config)
        {
            var item = config.Prototype.Clone();

            var res = inventory.Items.FirstOrDefault(i => i.Id == item.Id);

            if (res == null) return null;

            if (TryRemoveStackItem(inventory, res) == true) return res;

            inventory.Items.Remove(res);
            inventory.NotifyItemRemoved(res);

            return res;
        }

        public static void RemoveItems(Inventory inventory, Item item, int count)
        {
            for (int i = 0; i < count; i++)
            {
                RemoveItem(inventory, item);
            }
        }

        public static void ConsumeItem(Inventory inventory, Item item, bool consumeByRef = false)
        {
            if (item.CanConsume() == true)
            {
                var removed = RemoveItem(inventory, item, removeByRef: consumeByRef);
                if (removed == null) return;

                inventory.NotifyItemConsumed(removed);
            }
        }

        public static void ConsumeItem(Inventory inventory, ItemConfig itemConfig)
        {
            var proto = itemConfig.Prototype;

            if (proto.CanConsume() == true)
            {
                var removed = RemoveItem(inventory, itemConfig);
                if (removed == null) return;

                inventory.NotifyItemConsumed(removed);
            }
        }


        private static bool TryAddStackItem(Inventory inventory, Item item)
        {
            if (item.CanStack() == true)
            {
                foreach (var inventoryItem in inventory.Items)
                {
                    if (item.Id != inventoryItem.Id) continue;
                    if (inventoryItem.TryGetComponent(out StackableItemComponent component) == false) continue;
                    if (component.Count == component.MaxCount) continue;

                    component.Count++;

                    inventory.NotifyItemAddStacked(inventoryItem);
                    return true;
                }
            }

            return false;
        }

        private static bool TryRemoveStackItem(Inventory inventory, Item item)
        {
            if (item.CanStack() == true)
            {
                if (item.TryGetComponent(out StackableItemComponent component) == false) return false;

                if (component.Count <= 1) return false;

                component.Count--;

                inventory.NotifyItemRemoveStacked(item);
                return true;
            }

            return false;
        }
    }
}