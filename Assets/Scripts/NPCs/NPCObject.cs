using Mirror;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;


namespace NPC
{
    /* Script um mit NPCs zu interagieren 
     * 
     * 
     */
    public class NPCObject : NetworkBehaviour, IIinteractable
    {
        public NPCData npc;
        public DialogueRunner dialogue;
        public string[] dialogueFlow;
        public int currentDialogue = 0;

        public UnityEvent OnTalk;

        public string GetInteractPrompt()
        {
            if (PlayerController.GetPlayerSeperation() > 10f)
            {
                return string.Format("Ihr seid zu weit voneinander entfernt, um {0} anzusprechen!", npc.displayName);
            }
            return string.Format("{0} ansprechen", npc.displayName);
        }

        public void OnInteract(uint networkIdentifier)
        {
            OnTalk.Invoke();
            if (PlayerController.GetPlayerSeperation() <= 10f) StartDialogue();
        }

        [ClientRpc]
        private void StartDialogue()
        {
            var dia = GameObject.Find("Dialogue");
            if (dia.GetComponent<DialogueRunner>() != null)
            {
                NetworkClient.localPlayer.GetComponent<PlayerController>().SetActionMap("Dialogue");
                dia.GetComponent<DialogueRunner>().StartDialogue(dialogueFlow[currentDialogue]);
            }
        }

        [YarnCommand("set_dialogue")]
        public void SetDialogue(int key)
        {
            currentDialogue = key;
        }
    }
}

