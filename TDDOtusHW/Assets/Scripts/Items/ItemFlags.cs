using System;

namespace ATG.Items
{
    [Flags]
    public enum ItemFlags
    {
        None = 0,
        Stackable = 1,
        Consumable = 2,
        Equippable = 4,
        Effectable = 8
    }
}