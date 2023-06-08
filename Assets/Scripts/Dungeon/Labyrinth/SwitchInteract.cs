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
    [SerializeField] private GameObject doorOpen1,doorClose,doorOpen2;
    [SerializeField] private GameObject red_finish;
    
    [SerializeField] private Animator levlerAnimation = null;
    [SerializeField] private float doorScale = 700;

    private Renderer renderer;
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
        
        if (red_finish != null) renderer = red_finish.GetComponent<Renderer>();

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
        if (switchState == true && !toggle)
        {
            AnimateSwitchDown(); // Schalter Oben -> Unten
            if (renderer != null)
            {
                renderer.material.color = red;
            }
        }
        else if(!switchState && toggle)
        {
            AnimateSwitchUp(); // Schalter Unten -> Oben 
            if (renderer != null)
            {
                renderer.material.color = white;
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(levlerAnimation != null)_hasAnimator = true;
        if (opening)
        {
            OpenDoor();
            CloseDoor();
        }
        

    }

    
    private void OpenDoor()
    {
        if (!toggle) // Tür geht runter
        {
            if (doorOpen1 == null) return;
            if (doorOpen1.transform.localScale.z > 0)
            {
                doorOpen1.transform.localScale += new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorOpen1.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = true;
                opening = false;
            }

            if (doorOpen2 == null) return;
            if (doorOpen2.transform.localScale.z > 0)
            {
                doorOpen2.transform.localScale += new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorOpen2.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = true;
                opening = false;
            }
        }
        else
        {
            if (doorOpen1 == null) return;
            if (doorOpen1.transform.localScale.z < doorScale)
            {
                doorOpen1.transform.localScale -= new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorOpen1.transform.position -= new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = false;
                opening = false;
            }

            if (doorOpen2 == null) return;
            if (doorOpen2.transform.localScale.z < doorScale)
            {
                doorOpen2.transform.localScale -= new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorOpen2.transform.position -= new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = false;
                opening = false;
            }
        }
     
    }

    private void CloseDoor()
    {
        if (doorClose == null) return;
        if (!toggle)
        {
            if (doorClose.transform.localScale.z < doorScale)
            {
                doorClose.transform.localScale -= new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorClose.transform.position -= new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = true;
                opening = false;
            }
        }
        else
        {
            if (doorClose.transform.localScale.z > 0)
            {
                doorClose.transform.localScale += new Vector3(0, 0, -(1 * Time.deltaTime * speed));
                doorClose.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed / 200)), 0);
            }
            else
            {
                toggle = true;
                opening = false;
            }
        }
        

        
       

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
            levlerAnimation.Play("SwitchAnimation_Down", 0, 0.0f);
        }
        else
        {
            levlerAnimation.Play("SwitchAnimation_Up", 0, 0.0f);
        }
        
    }
    
   
}