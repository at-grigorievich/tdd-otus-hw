namespace ATG.Items
{
    public static class ItemExtensions
    {
        public static bool HasStaticEffect(this Item item)
        {
            return HasFlag(item, ItemFlags.Effectable);
        }
        
        public static bool CanConsume(this Item item)
        {
            return HasFlag(item, ItemFlags.Consumable);
        }

        public static bool CanStack(this Item item)
        {
            return HasFlag(item, ItemFlags.Stackable);
        }

        public static bool CanEquip(this Item item)
        {
            return HasFlag(item, ItemFlags.Equippable);
        }

        public static bool HasFlag(this Item item, ItemFlags itemFlag)
        {
            return (item.Flags & itemFlag) == itemFlag;
        }
    }
}