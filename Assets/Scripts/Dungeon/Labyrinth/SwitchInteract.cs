using Inventory;
using Mirror;
using Player;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;


/* Script um die T�ren im Labyrinth zu �ffnen 
 * 
 * bei Aktivierung des Schalters wird die T�r, die im Inspector festegelegt wird ge�ffnet 
 * Momentan wird die T�r f�r 10 Sekunden nach Untenbewegt ( Nicht sch�n ) 
 */

public class SwitchInteract : NetworkBehaviour, IIinteractable
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject lever;
    [SerializeField] private Animator levlerAnimation = null;
    



    private bool opening = false;
    private float counter = 0;
    private float speed = 30f;
  

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
            
            OpenDoor();
        }
      
    }
    
    [ClientRpc]
    private void OpenDoor()
    {
        if (door == null) return;
        if(door.transform.localScale.y > 0)
        {
            door.transform.localScale += new Vector3(0, -(1 * Time.deltaTime * speed), 0);
            door.transform.position += new Vector3(0, -(1 * Time.deltaTime * (speed/100)), 0); 
        }
    }
    private void AnimateSwitch()
    {
        if (lever == null && levlerAnimation == null) return;
        levlerAnimation.Play("Switch_Animation", 0, 0.0f);
        
    }
}
