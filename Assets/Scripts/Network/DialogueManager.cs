using Mirror;
using Player;
using UnityEngine;
using Yarn.Unity;

namespace Network
{
    public class DialogueManager : NetworkBehaviour
    {
        public DialogueRunner dialogueRunner;
        public DialogueViewBase[] activeDialogueViews;
        public DialogueViewBase[] passiveDialogueViews;
        
        public void EndDialogue()
        {
            if (isServer) EndDialogueRpc();
        }

        [ClientRpc]
        public void EndDialogueRpc()
        {
            NetworkClient.localPlayer.GetComponent<PlayerController>().SetActionMap("Player");
        }

        [ClientRpc]
        public void StartActiveDialogue(string startNode)
        {
            // Select the proper views
            MakeDialogueActive();
            // Prevent players from acting while dialogue is running
            NetworkClient.localPlayer.GetComponent<PlayerController>().SetActionMap("Dialogue");
            // Start the actual dialogue
            StartDialogue(startNode);
        }

        [ClientRpc]
        public void StartPassiveDialogue(string startNode)
        {
            // Select the proper views
            MakeDialoguePassive();
            // Start the actual dialogue
            StartDialogue(startNode);
        }

        private void StartDialogue(string startNode)
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue(startNode);
        }

        private void MakeDialoguePassive()
        {
            dialogueRunner.dialogueViews = passiveDialogueViews;
            passiveDialogueViews[0].gameObject.SetActive(true);
            activeDialogueViews[0].gameObject.SetActive(false);
        }

        private void MakeDialogueActive()
        {
            dialogueRunner.dialogueViews = activeDialogueViews;
            passiveDialogueViews[0].gameObject.SetActive(false);
            activeDialogueViews[0].gameObject.SetActive(true);
        }
    }
}
