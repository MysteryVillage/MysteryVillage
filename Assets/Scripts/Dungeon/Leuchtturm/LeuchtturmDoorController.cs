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

        void Start()
        {
            animator = GetComponent<Animator>();
            prompt = "Aufschließen";
            GetInteractPrompt();
        }

        public string GetInteractPrompt()
        {

            return string.Format(prompt);

        }

        public void OnInteract(uint networkIdentifier)
        {

            Door();
        }

        [ClientRpc]
        public void Door()
        {
            // evaluate inventory
            //var players = PlayerController.GetPlayers();
            //var collin = players[0].isBoy ? players[0] : players[1];

            //var inventory = InventoryManager.Instance().GetInventoryForPlayer(collin.gameObject);

            if (!isOpen)
            {
                //if (inventory.HasItem(ItemData.FindById(2357733091), 1))
                //{
                    isOpen = true;
                    //animator.SetBool("isOpen", isOpen);
                    if (animator != null) animator.Play("LT_Door_Animation_Open", 0, 0.0f); //Debug.Log(" Tür 1 Öffnet sich");
                    prompt = "Aufschließen";
                    GetInteractPrompt();

                    EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current as EventSystem;
                    if (eventSystem)
                    {
                        eventSystem.onQuestEvent.Invoke("LT_DoorUnlock");
                    }
                //}
                else
                {
                    prompt = "Diese Tür ist verschlossen";
                    GetInteractPrompt();
                }
            }
        }
    }
}

