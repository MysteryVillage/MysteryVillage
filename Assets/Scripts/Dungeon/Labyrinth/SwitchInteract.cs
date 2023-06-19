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

    [SerializeField] private Animator doorAnimationLeft = null;
    [SerializeField] private Animator doorAnimationRight = null;
    [SerializeField] private Animator levlerAnimation = null;
  

   
    private Color32 red = new Color32(197,73,73,255);
    private Color32 white = new Color32(227,227,227,255);

    private bool opening = false;
    private float speed = 80f;
    private bool switchState = true;
    private bool toggle = false;


    private bool _hasAnimator = false;
    private int _animID;
   


    private void Start()
    {
        
       

        if (levlerAnimation != null) _hasAnimator = true;
        _animID = Animator.StringToHash("Switch");
    }
    public string GetInteractPrompt()
    {
        return string.Format("Activate Switch {0}", "here");
    }

    public void OnInteract(uint networkIdentifier)
    {
        opening = true;
        // SwitchState true == wenn der Schalter oben ist
        // toggle false == wenn die Tür runter geht
        if (switchState == true )
        {
            AnimateSwitchDown(); // Schalter Oben -> Unten
            OpenDoor();
        }
        else if(!switchState )
        {
            AnimateSwitchUp(); // Schalter Unten -> Oben 
            CloseDoor();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
        

    }

    
    private void OpenDoor()
    {
        setDoorAnimation(true);
     
    }

    private void CloseDoor()
    {
        setDoorAnimation(false);

    }
    private void AnimateSwitchDown()
    {
        
        if ( levlerAnimation == null) return;
        
       // levlerAnimation.Play("SwitchAnimation_Down", 0, 0.0f);
        switchState = false; // Schalter ist unten 

        if (_hasAnimator)
        {
            setSwitchAnimation(true);

        }

    }
    private void AnimateSwitchUp()
    {
        if ( levlerAnimation == null) return;
        
        switchState = true; // Schalter ist oben 
       if (_hasAnimator)
        {
            setSwitchAnimation(false); // Animator Updaten
            

        }
    }
    
    [ClientRpc]
    private void setSwitchAnimation(bool SwitchDown)
    {
        if (SwitchDown)
        {
            levlerAnimation.Play("Switch_Down", 0, 0.0f);
            Debug.Log("Switch Down");
        }
        else
        {
            Debug.Log("Switch Up");
            levlerAnimation.Play("Switch_Up", 0, 0.0f);
        }
        
    }

    [ClientRpc]
    private void setDoorAnimation(bool DoorOpen)
    {
        if (DoorOpen)
        {
            doorAnimationLeft.Play("DoorLeft_Open", 0, 0.0f);
            doorAnimationRight.Play("DoorRight_Open", 0, 0.0f);
            Debug.Log("Tür Öffnen");
        }
        else
        {
            doorAnimationLeft.Play("DoorLeft_Close", 0, 0.0f);
            doorAnimationRight.Play("DoorRight_Close", 0, 0.0f);
            Debug.Log("Tür Schließen");
        }
    }
    
   
}