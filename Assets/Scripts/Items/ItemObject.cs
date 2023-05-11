using Mirror;
using UnityEngine;

namespace Items
{
    public class ItemObject : NetworkBehaviour, IIinteractable
    {
        public ItemData item;   

        public string GetInteractPrompt()
        {
            return string.Format("Pickup {0}", item.displayName);
        }

        public void OnInteract(uint networkIdentifier)
        {
            NetworkServer.Destroy(gameObject);
            GameObject player = NetworkServer.spawned[networkIdentifier].gameObject;
            PlayerInventory inventory = InventoryManager.Instance().Get(networkIdentifier);
            Debug.Log("Trying to add item " + item.displayName + " to player with netId " + networkIdentifier + " (" + player.name +")");
            inventory.AddItem(item);
        }
    }
}
