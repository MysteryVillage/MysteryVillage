using System;
using Mirror;
using Network;
using UI;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class Intro : NetworkBehaviour
    {
        private void Start()
        {
            Debug.Log("Start intro");
            
            FindObjectOfType<InMemoryVariableStorage>().SetValue("$debug", GameSettings.Get().isTestRun);
            
            // intro stuff
            
            FindObjectOfType<Crossfade>().FadeIn();
            
            if (isServer) FindObjectOfType<DialogueManager>().StartActiveDialogue("Intro");
        }
    }
}