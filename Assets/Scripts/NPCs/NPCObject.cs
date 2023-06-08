using Mirror;
using Player;
using UnityEngine;
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
        
        public string GetInteractPrompt()
        {
            return string.Format("Talk to {0}", npc.displayName);
        }

        public void OnInteract(uint networkIdentifier)
        {
            Debug.Log("ON INTERACT: " + isServer);
            StartDialogue();
        }

        [ClientRpc]
        private void StartDialogue()
        {
            Debug.Log("StartDialogue: " + isServer);
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

