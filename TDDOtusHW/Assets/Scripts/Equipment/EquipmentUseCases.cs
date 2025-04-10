namespace ATG.Items.Equipment
{
    public static class EquipmentUseCases
    {
        public static void TakeOnItem(Equipment equipment, Item item)
        {
            if(item.CanEquip() == false) return;
            
            if(item.TryGetComponent(out HeroEquipmentComponent component) == false) return;

            if (equipment.Items.ContainsKey(component.Tag) == true)
            {
                TakeOffItem(equipment, item);
            }
            
            equipment.Items.Add(component.Tag, item);
            equipment.ItemsDebug.Add(item);
            
            equipment.NotifyItemTakeOn(item);
        }

        public static Item TakeOffItem(Equipment equipment, Item item)
        {
            if(item.CanEquip() == false) return null;
            
            if(item.TryGetComponent(out HeroEquipmentComponent component) == false) return null;

            if (equipment.Items.ContainsKey(component.Tag) == true)
            {
                Item result = equipment.Items[component.Tag];
                
                equipment.Items.Remove(component.Tag);
                equipment.ItemsDebug.Remove(result);
                
                equipment.NotifyItemTakeOff(result);

                return result;
            }

            return null;
        }
    }
}