using System;
using ATG.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public readonly struct EquipmentViewData
{
    public readonly Item Item;

    public readonly string Id;
    public readonly Sprite Icon;

    public EquipmentViewData(Item item)
    {
        Item = item;

        Id = item.Id;
        Icon = item.MetaData.Icon;
    }
}

public class EquipmentItemView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image selectedVisual;

    public EquipmentViewData? Data { get; private set; }

    public event Action<EquipmentItemView> OnSelected;

    private void Awake()
    {
        Hide();
    }

    public void Show(EquipmentViewData data)
    {
        Data = data;
        icon.sprite = data.Icon;
        icon.enabled = true;
    }

    public void Hide()
    {
        Data = null;
        icon.enabled = false;

        SetSelectedStatus(false);
    }

    public void SetSelectedStatus(bool isSelected)
    {
        selectedVisual.enabled = isSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Data.HasValue == false) return;
        OnSelected?.Invoke(this);
    }
}