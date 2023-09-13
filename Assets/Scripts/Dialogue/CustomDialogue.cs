using System;
using Mirror;
using Network;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class CustomDialogue : NetworkBehaviour
    {
        public string startNode;
        public Type type;
        public float delay = 0;

        public void RunDialogue()
        {
            Invoke(nameof(Dialogue), delay);
        }

        public void Dialogue()
        {
            var dia = GameObject.Find("Dialogue");
            if (dia.GetComponent<DialogueRunner>() != null && isServer)
            {
                if (type == Type.Passive)
                {
                    dia.GetComponent<DialogueManager>().StartPassiveDialogue(startNode);
                }
                else
                {
                    dia.GetComponent<DialogueManager>().StartActiveDialogue(startNode);
                }
            }
        }

        public void Cancel()
        {
            CancelInvoke();
        }
    }

    public enum Type
    {
        Active,
        Passive
    }
}