using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class ServerInventory : NetworkBehaviour
{
    public readonly SyncDictionary<uint, InventoryData> Inventories = new ();

    [Header("Items")]
    public List<ItemData> availableItems = new();

    private static ServerInventory _instance;
    
    private ServerInventory()
    {
        _instance = this;
    }

    public InventoryData Get(uint netId)
    {
        return Inventories[netId];
    }

    public void RegisterInventory(uint netId)
    {
        NetworkIdentity player = NetworkServer.spawned[netId];
        InventoryData playerInventory = new InventoryData(16, player.gameObject);
        Inventories.Add(netId, playerInventory);
    }

    public static ServerInventory Instance()
    {
        return _instance;
    }

    public void AddItemToInventory(int itemId, uint netId)
    {
        print("Is server3: " + isServer);
        var inventory = Get(netId);
        inventory.AddItem(itemId);
    }

    public void RemoveItemFromInventoryByIndex(int index, uint netId)
    {
        print("Is server: " + isServer + "(RemoveItemFromInventoryByIndex)");
        var inventory = Get(netId);
        inventory.RemoveItemFromIndex(index);
    }
}

public class InventoryData
{
    public ClientInventory Inventory;
    public GameObject _player;
    public ItemSlot[] slots;

    public InventoryData() {}
    
    public InventoryData(int slotNumber, GameObject player)
    {
        slots = new ItemSlot[slotNumber];
        for (int x = 0; x < slotNumber; x++)
        {
            slots[x] = new ItemSlot();
        }
        _player = player;
        Inventory = _player.GetComponent<ClientInventory>();
    }

    ItemSlot GetItemStack(ItemData item)
    {
        for (int x = 0; x < slots.Length; x++)
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
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == null)
            {
                return slots[x];
            }
        }

        return null;
    }

    [Command]
    public void AddItem(int itemId)
    {
        ItemData item = ServerInventory.Instance().availableItems[itemId];
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);

            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                RefreshInventory();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            RefreshInventory();
            return;
        }
        
        // @TODO
        // inventory.ThrowItem(item);
    }

    public void RemoveItemFromIndex(int slotIndex)
    {
        Debug.Log(this);
        
        var selectedItem = slots[slotIndex];
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            selectedItem.item = null;
        }

        Debug.Log(this);
        
        RefreshInventory();
    }

    [Command]
    void RefreshInventory()
    {
        Debug.Log("Refresh Inventory");
        if (ServerInventory.Instance().isServer)
        {
            Debug.Log("Update Ui from Server");
            Inventory.UpdateUi(GetStrippedItemSlots());
        }
    }

    StrippedItemSlot[] GetStrippedItemSlots()
    {
        var strippedSlots = new StrippedItemSlot[slots.Length];
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                Debug.Log("Strip slot: " + slots[i].item.GetId());
                strippedSlots[i] = new StrippedItemSlot(slots[i].item.GetId(), slots[i].quantity);                
            }
            else
            {
                Debug.Log("Strip slot: empty");
                strippedSlots[i] = new StrippedItemSlot(-1, 0);
            }
        }

        return strippedSlots;
    }

    public override string ToString()
    {
        var text = "Items of player " + _player.name + ": \n";
        for (int i = 0; i < slots.Length; i++)
        {
            text += slots[i].ToString() + "\n";
        }

        return text;
    }
}

[Serializable]
public class StrippedItemSlot
{
    public int itemId;
    public int quantity;

    public StrippedItemSlot()
    {
        
    }

    public StrippedItemSlot(int itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }

    public ItemSlot Enrich()
    {
        var slot = new ItemSlot();
        slot.item = ItemData.FindById(itemId);
        slot.quantity = quantity;
        return slot;
    }
}
