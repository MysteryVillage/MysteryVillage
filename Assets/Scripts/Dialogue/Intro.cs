using System;
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
        
        private void Start()
        {
            Debug.Log("Start intro");
            
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$debug", GameSettings.Get().isTestRun);
            
            // intro stuff
            
            FindObjectOfType<Crossfade>().FadeIn();

            if (isServer)
            { 
                StartCoroutine(StartIntro());
            }
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
            
            FindObjectOfType<DialogueManager>().StartActiveDialogue("Intro");
        }
    }
}