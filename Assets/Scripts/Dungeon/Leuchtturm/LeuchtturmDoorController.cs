using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

using Mirror;
using Player;
using Inventory;
using Items;
using static Quests.Goals.BringToGoal;

namespace Environment.Buildings
{
    public class LeuchtturmDoorController : NetworkBehaviour, IIinteractable
    {
        private Animator animator;
        private bool isOpen = false;
        private string prompt;
        public ItemData key;
        private InventoryData inventory;
        private bool locked = true;

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
            if (locked)
            {
                if (GetInventory().HasItem(key, 1)) return "Tür aufschließen";
                return "Die Tür ist verschlossen.";
            }
            else
            {
                if (isOpen) return "Tür schließen";
                return "Tür öffnen";
            }
        }

        public void OnInteract(uint networkIdentifier)
        {
            Door();
        }

        [ClientRpc]
        public void Door()
        {
            // evaluate inventory
            if (locked)
            {
                if (GetInventory().HasItem(key, 1))
                {
                    isOpen = true;
                    // animator.SetBool("isOpen", isOpen);
                    if (animator != null) animator.Play("LT_Door_Animation_Open", 0, 0.0f);

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
                    if (animator != null) animator.Play("LT_Door_Animation_Close", 0, 0.0f);
                }
                else
                {
                    isOpen = true;
                    if (animator != null) animator.Play("LT_Door_Animation_Open", 0, 0.0f);
                }
            }
        }
    }
}

