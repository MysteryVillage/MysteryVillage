using Mirror;
using Player;
using UnityEngine;
using Yarn.Unity;

namespace Network
{
    public class DialogueManager : NetworkBehaviour
    {
        public DialogueRunner dialogueRunner;

        [ClientRpc]
        public void EndDialogue()
        {
            NetworkClient.localPlayer.GetComponent<PlayerController>().ToggleCursor(false);
        }
    }
}
