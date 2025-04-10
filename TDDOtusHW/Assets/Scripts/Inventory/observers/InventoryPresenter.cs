using System;

namespace ATG.Items.Inventory
{
    public sealed class InventoryPresenter: IInventoryObserver, IDisposable
    {
        private readonly Inventory _inventory;
        private readonly InventoryView _inventoryView;

        public InventoryPresenter(Inventory inventory, InventoryView inventoryView)
        {
            _inventory = inventory;
            _inventoryView = inventoryView;
            
            _inventory.OnItemAdded += OnItemAdded;
            _inventory.OnItemAddStacked += OnItemStacked;
            
            _inventory.OnItemRemoved += OnItemRemoved;
            _inventory.OnItemRemoveStacked += OnItemStacked;
            
            _inventoryView.OnConsumeClicked += OnConsumeClicked;
            _inventoryView.OnDropClicked += OnDropClicked;
        }
        
        public void Dispose()
        {
            _inventory.OnItemAdded -= OnItemAdded;
            _inventory.OnItemAddStacked -= OnItemStacked;
            
            _inventory.OnItemRemoved -= OnItemRemoved;
            _inventory.OnItemRemoveStacked -= OnItemStacked;
            
            _inventoryView.OnConsumeClicked -= OnConsumeClicked;
            _inventoryView.OnDropClicked -= OnDropClicked;
        }
        
        public void OnItemAdded(Item item)
        {
            _inventoryView.AddItem(item);
        }

        public void OnItemRemoved(Item item)
        {
            _inventoryView.RemoveItem(item, removeByRef: true);
        }
        
        private void OnItemStacked(Item obj)
        {
            _inventoryView.ChangeItem(obj);
        }
        
        private void OnDropClicked(Item obj)
        {
            InventoryUseCases.RemoveItem(_inventory, obj, removeByRef: true);
        }

        private void OnConsumeClicked(Item obj)
        {
            InventoryUseCases.ConsumeItem(_inventory, obj, consumeByRef: true);
        }
    }
}