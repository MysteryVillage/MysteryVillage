using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using StarterAssets;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Inventory : NetworkBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")] 
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject dropButton;

    private int curEquipIndex;
    
    // components
    private ThirdPersonController controller;

    [Header("Events")] 
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;


    private void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
    }

    private void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];
        
        // init the slots
        for (int x = 0; x < uiSlots.Length; x++)
        {
            uiSlots[x].index = x;
            uiSlots[x].Clear();
        }
        
        ClearSelectedItemWindow();
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }

    void UpdateUi()
    {
        for (int x = 0; x < uiSlots.Length; x++)
        {
            if (slots[x].item != null)
            {
                uiSlots[x].Set(slots[x]);
            }
            else
            {
                uiSlots[x].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData item)
    {
        for (int x = 0; x < uiSlots.Length; x++)
        {
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                return slots[x];
            }
        }
        
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int x = 0; x < uiSlots.Length; x++)
        {
            if (slots[x].item == null)
            {
                return slots[x];
            }
        }

        return null;
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null)
        {
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
        
        dropButton.SetActive(true);
    }

    [ClientRpc]
    public void ClearSelectedItemWindow()
    {
        // clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        
        // disable buttons
        dropButton.SetActive(false);
    }

    public void OnEquipButton(int index)
    {
        
    }

    public void OnUnequipButton()
    {
        
    }

    public void OnDropButton()
    {
        // @TODO
        // InventoryManager.Instance().ThrowItem(slots[selectedItemIndex].item, dropPosition);
        // InventoryManager.Instance().Get(GetComponent<NetworkIdentity>().netId).RemoveSelectedItem(slots[selectedItemIndex]);
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            if (uiSlots[selectedItemIndex].equipped == true)
            {
                UnEquip(selectedItemIndex);
            }

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
        
        UpdateUi();
    }

    void UnEquip(int i)
    {
        
    }

    public void RemoveItem(ItemData item)
    {
        
    }

    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }

    [ClientRpc]
    public void UpdateInventory(ItemSlot[] newSlots)
    {
        // var newSlots = InventoryManager.Instance().Get(GetComponent<NetworkIdentity>().netId).slots;
        for (int i = 0; i < newSlots.Length; i++)
        {
            if (newSlots[i].item)
            {
                var itemData = Resources.Load<ItemData>(newSlots[i].item.displayName);
                newSlots[i].item = itemData;
            }
            slots[i] = newSlots[i];

        }
        UpdateUi();
    }
}

[Serializable]
public class ItemSlot
{
    public ItemData item;
    public int quantity;
    public override string ToString()
    {
        if (!item)
        {
            return "Empty";
        }
        return item.displayName + "(x" + quantity + ")";
    }
}
