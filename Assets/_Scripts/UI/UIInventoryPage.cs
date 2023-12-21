using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private UIInventoryDescription itemDescription;

    [SerializeField]
    private MouseFollower mouseFollower;

    List<UIInventoryItem> listOfItems = new List<UIInventoryItem>();

    private int currentlyDraggedItemIndex = -1;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging; // OnDescriptionRequested: click event

    public event Action<int, int> OnSwapItems;

    public void initializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listOfItems.Add(uiItem);
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedon += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void Awake()
    {
        hideInventory();
        mouseFollower.Toggle(false);
        itemDescription.ResetDescription();
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfItems.Count > itemIndex)
        {
            listOfItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1)
            return;
        OnItemActionRequested?.Invoke(index);
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1)
            return;
        currentlyDraggedItemIndex = index;
        HandleItemSelection(item);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1)
            return;
        OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        HandleItemSelection(item);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentlyDraggedItemIndex -= 1;
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1)
            return;
        OnDescriptionRequested?.Invoke(index);
    }

    public void showInventory()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    public void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItem();
    }

    private void DeselectAllItem()
    {
        foreach (UIInventoryItem item in listOfItems)
        {
            item.DeSelect();
        }
    }

    public void hideInventory()
    {
        gameObject.SetActive(false);
        ResetDraggedItem();
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        DeselectAllItem();
        listOfItems[itemIndex].Select();
    }

    internal void ResetAllItems()
    {
        foreach (var item in listOfItems)
        {
            item.ResetData();
            item.DeSelect();
        }
    }
}
