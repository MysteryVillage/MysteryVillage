using Inventory;
using Mirror;
using Player;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;


/* Script um die T�ren im Labyrinth zu Öffnen 
 * 
 * bei Aktivierung des Schalters wird die Tür, die im Inspector festegelegt wird ge�ffnet 
 * Momentan wird die Tür für 10 Sekunden nach Untenbewegt ( Nicht sch�n ) 
 */

public class SwitchInteract : NetworkBehaviour, IIinteractable
{
<<<<<<< HEAD
    [SerializeField] private GameObject doorOpen1,doorClose,doorOpen2;
    [SerializeField] private GameObject red_finish;
    //[SerializeField] private GameObject lever;
    [SerializeField] private Animator levlerAnimation = null;

=======
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject lever;
    [SerializeField] private Animator levlerAnimation = null;
    
>>>>>>> main



    private bool opening = false;
    private float counter = 0;
    private float speed = 30f;
<<<<<<< HEAD

=======
  
>>>>>>> main

    public string GetInteractPrompt()
    {
        return string.Format("Activate Switch {0}", "here");
    }

    public void OnInteract(uint networkIdentifier)
    {
        opening = true;
        AnimateSwitch();
        Debug.Log("Schalter betätigt");
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
<<<<<<< HEAD

=======
            
>>>>>>> main
            OpenDoor();
        }

    }

    [ClientRpc]
    private void OpenDoor()
    {
<<<<<<< HEAD
        if (doorOpen1 == null) return;
        if (doorOpen1.transform.localScale.y > 0)
        {
            doorOpen1.transform.localScale += new Vector3(0, -(1 * Time.deltaTime * speed), 0);
            doorOpen1.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed / 100)), 0);
=======
        if (door == null) return;
        if(door.transform.localScale.y > 0)
        {
            door.transform.localScale += new Vector3(0, -(1 * Time.deltaTime * speed), 0);
            door.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed/100)), 0); 
>>>>>>> main
        }
    }
    private void AnimateSwitch()
    {
<<<<<<< HEAD
        if ( levlerAnimation == null) return;
        levlerAnimation.Play("SwitchAnimation", 0, 0.0f);

    }
}
=======
        if (lever == null && levlerAnimation == null) return;
        levlerAnimation.Play("Switch_Animation", 0, 0.0f);
        
    }
}
>>>>>>> main
