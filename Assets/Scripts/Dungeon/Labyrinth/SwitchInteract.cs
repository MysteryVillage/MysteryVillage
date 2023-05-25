using Inventory;
using Mirror;
using Player;
using UnityEngine;


/* Script um die T�ren im Labyrinth zu �ffnen 
 * 
 * bei Aktivierung des Schalters wird die T�r, die im Inspector festegelegt wird ge�ffnet 
 * Momentan wird die T�r f�r 10 Sekunden nach Untenbewegt ( Nicht sch�n ) 
 */

public class SwitchInteract : NetworkBehaviour, IIinteractable
{
    [SerializeField] private GameObject door;
    private bool opening = false;
    private float counter = 0;

    public string GetInteractPrompt()
    {
        return string.Format("Activate Switch {0}", "here");
    }
    
    public void OnInteract(uint networkIdentifier)
    {
        opening = true;
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
        
        Vector3 down = new Vector3(0, 0, 0);
        down.y -= 1 * Time.deltaTime;
        door.transform.position += down;
        counter += 1 * Time.deltaTime;
        if(counter > 10)
        {
            opening = false;
            counter = 0;
        }
    }
}
