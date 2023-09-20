using Events;
using Inventory;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public class Book : NetworkBehaviour, IIinteractable
    {
        public ItemData item;

        public string GetInteractPrompt()
        {
            if (NetworkClient.localPlayer.GetComponent<PlayerController>().isBoy)
            {
                return $"Nur Alina kann {item.displayName} einsammeln.";
            }
            return $"{item.displayName} einsammeln";
        }

        public void OnInteract(uint networkIdentifier)
        {
            var player = NetworkServer.spawned[networkIdentifier].gameObject;
            if (player.GetComponent<PlayerController>().isBoy) return;
            NetworkServer.Destroy(gameObject);
            player.GetComponent<PlayerController>().hasBook = true;
            
            EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
            if (eventSystem)
            {
                eventSystem.onQuestEvent.Invoke("bookCollected");
            }
        }
    }
}