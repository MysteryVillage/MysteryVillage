using System;
using Mirror;
using Network;
using UI;
using UnityEngine;

namespace Dialogue
{
    public class Intro : NetworkBehaviour
    {
        private void Start()
        {
            Debug.Log("Start intro");
            
            // intro stuff
            
            FindObjectOfType<Crossfade>().FadeIn();
            
            FindObjectOfType<DialogueManager>().StartActiveDialogue("Intro");
        }
    }
}