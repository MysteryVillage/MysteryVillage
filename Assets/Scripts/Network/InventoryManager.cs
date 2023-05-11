using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryManager : NetworkBehaviour
{
    public readonly SyncDictionary<uint, PlayerInventory> Inventories = new ();

    private static InventoryManager _instance;

    private InventoryManager()
    {
        _instance = this;
    }

    public static InventoryManager Instance()
    {
        return _instance;
    }

    public void RegisterInventory(uint networkIdentifier)
    {
        NetworkIdentity player = NetworkServer.spawned[networkIdentifier];
        PlayerInventory playerInventory = new PlayerInventory(16, player.gameObject);
        Inventories.Add(networkIdentifier, playerInventory);
    }

    public PlayerInventory Get(uint networkIdentifier)
    {
        return Inventories[networkIdentifier];
    }

    [Command]
    public void ThrowItem(ItemData item, Transform dropPosition)
    {
        var newItem = Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
        NetworkServer.Spawn(newItem);
    }
}

[Serializable]
public class PlayerInventory
{
    public ItemSlot[] slots;
    public Inventory inventory;
    private GameObject _player;

    public PlayerInventory()
    {
        
    }

    public PlayerInventory(int slotNumber, GameObject player)
    {
        slots = new ItemSlot[slotNumber];
        for (int x = 0; x < slotNumber; x++)
        {
            slots[x] = new ItemSlot();
        }
        _player = player;
        inventory = _player.GetComponent<Inventory>();
    }
    
    public void AddItem(ItemData item)
    {
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

    public override string ToString()
    {
        if (slots[0] != null)
        {
            return slots[0].item ? slots[0].item.displayName : _player.name;
        }
        return _player.name;
    }

    [Command]
    public void RefreshInventory()
    {
        if (InventoryManager.Instance().isServer) {
            inventory.UpdateInventory(slots);
        }
    }

    public void RemoveSelectedItem(ItemSlot selectedItem)
    {
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            selectedItem.item = null;
            inventory.ClearSelectedItemWindow();
        }
        
        RefreshInventory();
    }
}
