using ATG.Items;
using ATG.Items.Equipment;
using ATG.Items.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

public sealed class InventoryItemDebug : MonoBehaviour
{
    [SerializeField] private ItemConfig[] initialItems;

    [SerializeField] private Inventory inventory;
    [SerializeField] private Equipment equipment;

    [SerializeField] private Hero hero;

    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private EquipmentSetView equipmentView;

    private HeroItemsEffectsController _heroItemsEffectsController;
    private HeroConsumeEffectsObserver _heroConsumeEffectsObserver;

    private HeroEquipEffectObserver _heroEquipEffectObserver;

    private InventoryPresenter _inventoryPresenter;
    private EquipmentPresenter _equipmentPresenter;

    private InventoryToEquipmentProvider _provider;

    private void Awake()
    {
        _heroItemsEffectsController = new HeroItemsEffectsController(inventory, hero);
        _heroConsumeEffectsObserver = new HeroConsumeEffectsObserver(inventory, hero);

        _inventoryPresenter = new InventoryPresenter(inventory, inventoryView);
        _equipmentPresenter = new EquipmentPresenter(equipment, equipmentView);

        _provider = new InventoryToEquipmentProvider(inventory, equipment, inventoryView, equipmentView);

        _heroEquipEffectObserver = new HeroEquipEffectObserver(equipment, hero);
    }

    private void Start()
    {
        foreach (var inventoryItem in initialItems)
        {
            AddItem(inventoryItem);
        }
    }

    private void OnDestroy()
    {
        _heroItemsEffectsController.Dispose();
        _heroConsumeEffectsObserver.Dispose();

        _inventoryPresenter.Dispose();
        _equipmentPresenter.Dispose();

        _provider.Dispose();

        _heroConsumeEffectsObserver.Dispose();
        _heroEquipEffectObserver.Dispose();
    }

    [Button]
    public void AddItem(ItemConfig config)
    {
        var item = config.Prototype.Clone();
        InventoryUseCases.AddItem(inventory, item);
    }

    [Button]
    public void RemoveItem(ItemConfig config)
    {
        var item = config.Prototype.Clone();
        InventoryUseCases.RemoveItem(inventory, item);
    }
}