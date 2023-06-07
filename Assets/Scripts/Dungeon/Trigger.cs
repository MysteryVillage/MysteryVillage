using Mirror;
using Player;
using UnityEngine.Events;

namespace Dungeon
{
    public class Trigger : NetworkBehaviour, IIinteractable
    {
        public UnityEvent onInteract;
        public string triggerName;

        public string GetInteractPrompt()
        {
            return string.Format("Trigger {0}", triggerName);
        }

        public void OnInteract(uint networkIdentifier)
        {
            onInteract.Invoke();
        }
    }
}