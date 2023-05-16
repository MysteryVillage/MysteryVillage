using System;
using System.Collections.Generic;
using Items;
using Mirror;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : NetworkBehaviour
    {
        public readonly SyncDictionary<uint, InventoryData> Inventories = new ();

        [Header("Items")]
        public List<ItemData> availableItems = new();

        private static InventoryManager _instance;
    
        private InventoryManager()
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

        public static InventoryManager Instance()
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
}