using System;
using ATG.Items;
using ATG.OtusHW.Inventory;
using UnityEngine;
using UnityEngine.UI;

public sealed class EquipmentSetView : MonoBehaviour
{
    [SerializeField] private EquipmentPlaceholder[] placeholders;
    [SerializeField] private Button takeOffButton;

    private EquipmentItemView _selectedItemView;

    public event Action<Item> OnItemTakeOffClicked;

    private void Awake()
    {
        ClearSelectedItemView();
    }

    private void OnEnable()
    {
        foreach (var placeholder in placeholders)
        {
            placeholder.OnSelected += OnSelected;
        }

        takeOffButton.onClick.AddListener(OnSelectedTakeOff);
    }

    private void OnDisable()
    {
        foreach (var placeholder in placeholders)
        {
            placeholder.OnSelected -= OnSelected;
        }

        takeOffButton.onClick.RemoveListener(OnSelectedTakeOff);
    }

    public void AddItem(Item item)
    {
        if (item.TryGetComponent(out HeroEquipmentComponent equipment) == false) return;

        EquipmentItemView needItemView = GetViewByTag(equipment.Tag);
        needItemView.Show(new EquipmentViewData(item));
    }

    public void RemoveItem(Item item)
    {
        if (item.TryGetComponent(out HeroEquipmentComponent equipment) == false) return;

        EquipmentItemView needItemView = GetViewByTag(equipment.Tag);

        if (needItemView.Data.HasValue == false) return;

        if (ReferenceEquals(_selectedItemView, needItemView) == true)
        {
            ClearSelectedItemView();
        }

        needItemView.Hide();
    }

    private void OnSelected(EquipmentItemView obj)
    {
        if (_selectedItemView != null)
        {
            _selectedItemView.SetSelectedStatus(false);
        }

        _selectedItemView = obj;
        _selectedItemView.SetSelectedStatus(true);

        takeOffButton.gameObject.SetActive(true);
    }

    private void OnSelectedTakeOff()
    {
        if (_selectedItemView == null) return;
        OnItemTakeOffClicked?.Invoke(_selectedItemView.Data!.Value.Item);

        ClearSelectedItemView();
    }

    private void ClearSelectedItemView()
    {
        if (_selectedItemView != null)
        {
            _selectedItemView.SetSelectedStatus(false);
        }

        takeOffButton.gameObject.SetActive(false);

        _selectedItemView = null;
    }

    private EquipmentItemView GetViewByTag(EquipType tag)
    {
        foreach (var p in placeholders)
        {
            if (p.Tag == tag) return p.ItemView;
        }

        throw new NullReferenceException($"No view for {tag}... Check placeholders");
    }

    [Serializable]
    private sealed class EquipmentPlaceholder
    {
        [field: SerializeField] public EquipType Tag { get; private set; }
        [field: SerializeField] public EquipmentItemView ItemView { get; private set; }

        public event Action<EquipmentItemView> OnSelected
        {
            add => ItemView.OnSelected += value;
            remove => ItemView.OnSelected -= value;
        }
    }
}