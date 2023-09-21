using System.Collections;
using Mirror;
using Network;
using UI;
using UnityEngine;
using Yarn.Unity;
using NetworkManager = Network.NetworkManager;

namespace Dialogue
{
    public class Intro : NetworkBehaviour
    {
        private bool ready = false;
        [SyncVar] private bool skipIntro;
        [SyncVar] private int startQuest;
        
        private void Start()
        {
            if (isServer)
            {
                skipIntro = GameSettings.Get().isTestRun;
                startQuest = GameSettings.Get().startQuest;
                StartCoroutine(StartIntro());
            }
            
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$debug", skipIntro);
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$startQuest", startQuest);
            
            FindObjectOfType<Crossfade>().FadeIn();
        }
        
        public void ReadyUp()
        {
            ready = true;
        }

        private IEnumerator StartIntro()
        {
            var playercount = NetworkManager.singleton.roomSlots.Count;
            Debug.Log("Room players: " + playercount);
            if (playercount > 1) {
                yield return new WaitUntil(() => ready);
            }
            
            FindObjectOfType<DialogueManager>().StartActiveDialogue("StartGame");
        }
    }
}