using System;
using Events;
using Inventory;
using Mirror;
using Player;
using Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class ItemObject : NetworkBehaviour, IIinteractable
    {
        public ItemData item;
        public EventSystem eventSystem;

        public UnityEvent<ItemData> OnCollect;

        private void Start()
        {
            eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
        }

        public string GetInteractPrompt()
        {
            return $"Pickup {item.displayName}";
        }

        public void OnInteract(uint networkIdentifier)
        {
            NetworkServer.Destroy(gameObject);
            var player = NetworkServer.spawned[networkIdentifier].gameObject;
            
            Debug.Log("Trying to add item " + item.displayName + " to player with netId " + networkIdentifier + " (" + player.name +")");
            player.GetComponent<PlayerInventory>().CollectItem(item.GetId());

            if (eventSystem) eventSystem.OnItemPickup.Invoke(item);
        }
    }
}
