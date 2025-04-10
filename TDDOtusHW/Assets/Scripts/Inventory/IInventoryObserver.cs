namespace ATG.Items.Inventory
{
    public interface IInventoryObserver
    {
        void OnItemAdded(Item item);
        void OnItemRemoved(Item item);
    }
}