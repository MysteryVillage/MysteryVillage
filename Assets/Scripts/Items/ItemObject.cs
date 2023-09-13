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

        public SpawnArea spawnArea;

        public UnityEvent<ItemData> OnCollect;

        private void Start()
        {
            eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
        }

        public string GetInteractPrompt()
        {
            if (!NetworkClient.localPlayer.GetComponent<PlayerController>().isBoy)
            {
                return $"Nur Collin kann {item.displayName} einsammeln.";
            }
            return $"{item.displayName} einsammeln";
        }

        public void OnInteract(uint networkIdentifier)
        {
            var player = NetworkServer.spawned[networkIdentifier].gameObject;
            if (!player.GetComponent<PlayerController>().isBoy) return;
            NetworkServer.Destroy(gameObject);
            
            Debug.Log("Trying to add item " + item.displayName + " to player with netId " + networkIdentifier + " (" + player.name +")");
            player.GetComponent<PlayerInventory>().CollectItem(item.GetId());

            if (eventSystem) eventSystem.OnItemPickup.Invoke(item);
            
            // Tell spawn area that something was picked up
            // var spawnArea = SpawnArea.GetSpawnArea(transform.position, item);
            if (spawnArea != null)
            {
                spawnArea.itemCount--;
            }
        }
    }
}
