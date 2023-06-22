using Inventory;
using Mirror;
using Player;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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
    [SerializeField] private Sprite switchUp, switchDown, doorOpen, doorClose;
    [SerializeField] private Image switchImage, doorImage;

   
    private Color32 red = new Color32(197,73,73,255);
    private Color32 white = new Color32(227,227,227,255);

    private bool opening = false;
    private float speed = 80f;
    private bool switchState = true;
    private bool toggle = false;


    private bool _hasAnimator = false;
    private int _animID;

    private Gamepad gamepad;
    private Coroutine stopRumbleAfterTime;

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
            setSwitchandDoorImage(true); // Tür ist offen und Schalter unten
        }
        else if(!switchState )
        {
            AnimateSwitchUp(); // Schalter Unten -> Oben 
            CloseDoor();
            setSwitchandDoorImage(false); // Tür ist zu und Schalter oben 
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
    private void setSwitchandDoorImage(bool state)
    {
        if (state)
        {
           if(doorImage != null && doorOpen != null)    doorImage.sprite = doorOpen;
           if(switchImage != null && switchDown != null)  switchImage.sprite = switchDown;
        }
        else
        {
            if(doorImage != null && doorClose != null)  doorImage.sprite = doorClose;
            if(switchImage != null && switchUp != null) switchImage.sprite = switchUp;

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
            RumblePulse(0.15f, 0.15f, 1f);
            if (doorAnimationRight == null || doorAnimationLeft == null) return;
            
            doorAnimationLeft.Play("DoorLeft_Open", 0, 0.0f);
            doorAnimationRight.Play("DoorRight_Open", 0, 0.0f);
            Debug.Log("Tür Öffnen");
        }
        else
        {
            RumblePulse(0.15f, 0.15f, 1f);
            if (doorAnimationRight == null || doorAnimationLeft == null) return;
            
            doorAnimationLeft.Play("DoorLeft_Close", 0, 0.0f);
            doorAnimationRight.Play("DoorRight_Close", 0, 0.0f);
            Debug.Log("Tür Schließen");
        }
    }
    
    private void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        gamepad = Gamepad.current;
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            stopRumbleAfterTime = StartCoroutine(StopRumble(duration, gamepad));
        }
        else Debug.Log("Kein GamePad Angeschlossen");
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapesedTime = 0f;
        while (elapesedTime < duration)
        {
            elapesedTime += Time.deltaTime;
            yield return null;
        }

        pad.SetMotorSpeeds(0f, 0f);
    }
   
}