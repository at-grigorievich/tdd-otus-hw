namespace ATG.Items.Equipment
{
    public interface IEquipmentObserver
    {
        void OnItemTakeOn(Item item);
        void OnItemTakeOff(Item item);
    }
}