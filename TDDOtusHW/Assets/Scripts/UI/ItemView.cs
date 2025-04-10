using System;
using ATG.Items;
using ATG.OtusHW.Inventory.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public readonly struct ItemViewData
{
    public readonly Item Item;
    
    public readonly string Id;
    public readonly string Name;
    public readonly Sprite Icon;
        
    public readonly bool IsStackable;
    public readonly int StackCurrent;
    public readonly int StackMax;

    public readonly bool IsConsumable;
    public readonly bool IsEquipable;

    public ItemViewData(Item item)
    {
        Item = item;
        
        Id = item.Id;
        Name = item.MetaData.Name;
        Icon = item.MetaData.Icon;

        IsStackable = item.CanStack();
        IsConsumable = item.CanConsume();
        IsEquipable = item.CanEquip();

        if (IsStackable == true && item.TryGetComponent(out StackableItemComponent component) == true)
        {
            StackCurrent = component.Count;
            StackMax = component.MaxCount;
        }
        else
        {
            StackCurrent = StackMax = 0;
        }
    }
}

public class ItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text itemNameOutput;
    [SerializeField] private Image icon;
    [SerializeField] private CounterView counter;
    [SerializeField] private Image selectedVisual;
    
    public event Action<ItemView> OnSelected;
    
    public ItemViewData Data { get; private set; }
    
    private void Awake()
    {
        counter.SetActive(false);
        SetSelectedStatus(false);
    }

    public void Show(ItemViewData data)
    {
        Data = data;

        itemNameOutput.text = data.Name;
        
        icon.sprite = data.Icon;
        
        counter.SetActive(data.IsStackable);
        counter.UpdateCount(data.StackCurrent, data.StackMax);
        
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        counter.SetActive(false);

        Data = default;
    }

    public void SetSelectedStatus(bool isSelected)
    {
        selectedVisual.enabled = isSelected;
    }
    
    public void OnPointerClick(PointerEventData eventData) => OnSelected?.Invoke(this);
    
}
