using System;
using System.Collections;
using Mirror;
using Network;
using UI;
using UnityEngine;
using Yarn.Unity;

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
            yield return new WaitUntil(() => ready);
            
            FindObjectOfType<DialogueManager>().StartActiveDialogue("Intro");
        }
    }
}