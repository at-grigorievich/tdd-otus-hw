using System;
using System.Collections.Generic;
using System.Linq;
using ATG.Items;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class InventoryViewPool
{
    [SerializeField] private RectTransform itemsGrid;
    [SerializeField] private ItemView prefab;
    [SerializeField] private int poolSize;

    private Queue<ItemView> _pool = new();

    public void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            _pool.Enqueue(CreateInstance());
        }
    }

    public void Enqueue(ItemView itemView)
    {
        itemView.Hide();
        _pool.Enqueue(itemView);
    }

    public ItemView Dequeue()
    {
        if (_pool.Count <= 0)
        {
            _pool.Enqueue(CreateInstance());
        }

        return _pool.Dequeue();
    }

    private ItemView CreateInstance()
    {
        ItemView instance = GameObject.Instantiate(prefab, itemsGrid);
        instance.Hide();

        return instance;
    }
}

public sealed class InventoryView : MonoBehaviour
{
    [SerializeField] private InventoryViewPool pool;
    [Space(10)] [SerializeField] private Button dropBtn;
    [SerializeField] private Button consumeBtn;
    [SerializeField] private Button equipBtn;

    private HashSet<ItemView> _activeItems = new HashSet<ItemView>();

    private ItemView _lastSelected;

    public event Action<Item> OnDropClicked;
    public event Action<Item> OnConsumeClicked;
    public event Action<Item> OnEquipClicked;

    private void Awake()
    {
        pool.InitPool();
        SetupConsumeButton(false);
        SetupEquipButton(false);
        SetupDropButton(false);
    }

    private void OnEnable()
    {
        dropBtn.onClick.AddListener(OnDropClickedHandler);
        consumeBtn.onClick.AddListener(OnConsumeClickedHandler);
        equipBtn.onClick.AddListener(OnEquipClickedHandler);
    }

    private void OnDisable()
    {
        dropBtn.onClick.RemoveAllListeners();
        consumeBtn.onClick.RemoveAllListeners();
        equipBtn.onClick.RemoveAllListeners();
    }

    public void AddItem(Item item)
    {
        ItemViewData viewData = new ItemViewData(item);

        ItemView view = pool.Dequeue();
        view.Show(viewData);

        view.OnSelected += OnSelectedView;

        _activeItems.Add(view);
    }

    public void ChangeItem(Item item)
    {
        ItemViewData viewData = new ItemViewData(item);

        ItemView selectedView = GetActive(item, true);

        selectedView.Show(viewData);
    }

    public void RemoveItem(Item item, bool removeByRef = false)
    {
        ItemView removedView = GetActive(item, removeByRef);

        removedView.OnSelected -= OnSelectedView;

        _activeItems.Remove(removedView);
        pool.Enqueue(removedView);

        if (ReferenceEquals(removedView, _lastSelected))
        {
            _lastSelected.SetSelectedStatus(false);

            SetupConsumeButton(false);
            SetupEquipButton(false);
            SetupDropButton(false);

            _lastSelected = null;
        }
    }

    private void OnSelectedView(ItemView obj)
    {
        if (_lastSelected != null)
        {
            _lastSelected.SetSelectedStatus(false);
        }

        obj.SetSelectedStatus(true);

        _lastSelected = obj;

        SetupConsumeButton(_lastSelected.Data.IsConsumable);
        SetupEquipButton(_lastSelected.Data.IsEquipable);
        SetupDropButton(true);
    }

    private void SetupDropButton(bool isActive) => dropBtn.gameObject.SetActive(isActive);
    private void SetupConsumeButton(bool isActive) => consumeBtn.gameObject.SetActive(isActive);
    private void SetupEquipButton(bool isActive) => equipBtn.gameObject.SetActive(isActive);

    private void OnDropClickedHandler()
    {
        if (_lastSelected == null) return;
        OnDropClicked?.Invoke(_lastSelected.Data.Item);
    }

    private void OnConsumeClickedHandler()
    {
        if (_lastSelected == null) return;

        OnConsumeClicked?.Invoke(_lastSelected.Data.Item);
    }

    private void OnEquipClickedHandler()
    {
        if (_lastSelected == null) return;

        OnEquipClicked?.Invoke(_lastSelected.Data.Item);
    }

    private ItemView GetActive(Item item, bool findByRef)
    {
        ItemView result = _activeItems.FirstOrDefault(i => findByRef == false
            ? i.Data.Id == item.Id
            : ReferenceEquals(item, i.Data.Item));

        if (result == null)
            throw new NullReferenceException($"Item {item.Id} was not found in inventory view");

        return result;
    }
}