using Mirror;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : NetworkBehaviour, IIinteractable
{
    private Animator animator;
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInteractPrompt();
    }

    public string GetInteractPrompt()
    {
        
        if (!isOpen)
        {
            return string.Format("÷ffnen");
        }
        else
        {
            return string.Format("Schlieﬂen");
        }
    }

    public void OnInteract(uint networkIdentifier)
    {
        if(!isOpen) { 
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
        }
        else
        {
            isOpen = false;
            animator.SetBool("isOpen", isOpen);
        }
       
    }
}
