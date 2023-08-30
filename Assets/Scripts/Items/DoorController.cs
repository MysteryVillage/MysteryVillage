using Mirror;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : NetworkBehaviour, IIinteractable
{
    private Animator animator;
    private bool isOpen = false;
    private string prompt;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        prompt = "÷ffnen";
        GetInteractPrompt();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public string GetInteractPrompt()
    {
        
            return string.Format(prompt);
    
    }

    public void OnInteract(uint networkIdentifier)
    {

        Door();
    }

    [ClientRpc]
    public void Door()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
            prompt = "Schlieﬂen";
            GetInteractPrompt();
        }
        else
        {
            isOpen = false;
            animator.SetBool("isOpen", isOpen);
            prompt = "÷ffnen";
            GetInteractPrompt();
        }
    }
}
