using Events;
using Inventory;
using Items;
using Mirror;
using Player;
using UnityEngine;

namespace Dungeon.Leuchtturm
{
    public class LeuchtturmDoorController : NetworkBehaviour, IIinteractable
    {
        private Animator animator;
        private bool isOpen = false;
        private string prompt;
        public ItemData key;
        private InventoryData inventory;
        private bool locked = true;
        [SyncVar]
        private bool keyAvailable;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        InventoryData GetInventory()
        {
            if (inventory == null)
            {
                var players = PlayerController.GetPlayers();
                var collin = players[0].isBoy ? players[0] : players[1];
                
                inventory = InventoryManager.Instance().GetInventoryForPlayer(collin.gameObject);
            }
            return inventory;
        }

        public string GetInteractPrompt()
        {
            CheckForKey();
            if (locked)
            {
                if (keyAvailable) return "Tür aufschließen";
                return "Die Tür ist verschlossen.";
            }
            else
            {
                if (isOpen) return "Tür schließen";
                return "Tür öffnen";
            }
        }

        [Command]
        public void CheckForKey()
        {
            keyAvailable = GetInventory().HasItem(key, 1);
        }

        public void OnInteract(uint networkIdentifier)
        {
            Door();
        }
        
        public void Door()
        {
            // evaluate inventory
            if (locked)
            {
                if (GetInventory().HasItem(key, 1))
                {
                    isOpen = true;
                    // animator.SetBool("isOpen", isOpen);
                    OpenDoor();

                    locked = false;

                    EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
                    if (eventSystem)
                    {
                        eventSystem.onQuestEvent.Invoke("LT_DoorUnlock");
                    }
                }
            }
            else
            {
                if (isOpen)
                {
                    isOpen = false;
                    CloseDoor();
                }
                else
                {
                    isOpen = true;
                    OpenDoor();
                }
            }
        }

        [ClientRpc]
        public void OpenDoor()
        {
            if (animator != null) animator.Play("LT_Door_Animation_Open", 0, 0.0f);
        }
        [ClientRpc]
        public void CloseDoor()
        {
            if (animator != null) animator.Play("LT_Door_Animation_Close", 0, 0.0f);
        }
    }
}

