using Inventory;
using Mirror;
using Player;
using UnityEngine;


/* Script um die Türen im Labyrinth zu öffnen 
 * 
 * bei Aktivierung des Schalters wird die Tür, die im Inspector festegelegt wird geöffnet 
 * Momentan wird die Tür für 10 Sekunden nach Untenbewegt ( Nicht schön ) 
 */

public class SwitchInteract : MonoBehaviour, IIinteractable
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            OpenDoor();
            counter += 1 * Time.deltaTime;
        }
      
    }
    private void OpenDoor()
    {
        Vector3 down = new Vector3(0, 0, 0);
        down.y -= 1 * Time.deltaTime;
        door.transform.position += down;
        if(counter > 10)
        {
            opening = false;
            counter = 0;
        }
    }
}
