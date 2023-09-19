using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

using Mirror;
using Player;

namespace Environment.Buildings
{
    public class LeuchtturmDoorController : NetworkBehaviour, IIinteractable
    {
        private Animator animator;
        private bool isOpen = false;
        private string prompt;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            prompt = "Aufschließen";
            GetInteractPrompt();
        }

        // Update is called once per frame
        void Update()
        {

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
            if (!isOpen)
            {
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
            }
        }
    }
}

