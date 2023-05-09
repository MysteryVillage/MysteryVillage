using Mirror;

namespace Items
{
    public class ItemObject : NetworkBehaviour, IIinteractable
    {
        public ItemData item;   

        public string GetInteractPrompt()
        {
            return string.Format("Pickup {0}", item.displayName);
        }

        public void OnInteract()
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
