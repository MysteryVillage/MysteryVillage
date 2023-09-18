using System;
using System.Collections.Generic;
using Items;
using Mirror;
using Player;
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

        public void ClearInventories()
        {
            Inventories.Clear();
        }

        public static InventoryManager Instance()
        {
            return _instance;
        }

        public InventoryData GetInventoryForPlayer(GameObject player)
        {
            foreach (var inventory in Inventories)
            {
                if (inventory.Value._player == player)
                {
                    return inventory.Value;
                }
            }

            return null;
        }

        public void AddItemToInventory(int itemId, uint netId)
        {
            // Retrieve inventory component by netId
            var inventory = Get(netId);
            
            // Retrieve connectionId by inventory component
            var connId = inventory.Inventory.GetComponent<NetworkIdentity>().connectionToClient.connectionId;
            
            // Add item to inventory
            inventory.AddItem(itemId);
            
            // Notify client of item pickup
            PickupNotification(NetworkServer.connections[connId], itemId);
        }

        [TargetRpc]
        public void PickupNotification(NetworkConnectionToClient target, int itemId)
        {
            target.identity.GetComponent<PlayerController>().playerUi.PickupNotification(itemId);
        }

        public void RemoveItemFromInventoryByIndex(int index, uint netId)
        {
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