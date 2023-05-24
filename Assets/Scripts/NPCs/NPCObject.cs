using Inventory;
using Items;
using Mirror;
using Player;
using System.Threading;
using TMPro;
using UnityEngine;


namespace NPC
{
    /* Script um mit NPCs zu interagieren 
     * 
     * 
     */
    public class NPCObject : MonoBehaviour, IIinteractable
    {
        public NPCData npc;
        [SerializeField] private GameObject dialogWindow;
        private TMP_Text dialogText;
        private bool start_timer=false;
        private float timer;
       
        

        private void Awake()
        {
            dialogText = GameObject.Find("NPCText").GetComponent<TMP_Text>();

            
            dialogWindow.SetActive(false);
        }
        private void Start()
        {
            
        }
        public string GetInteractPrompt()
        {
            return string.Format("Talk to {0}", npc.displayName);
        }

        public void OnInteract(uint networkIdentifier)
        {
          
            dialogWindow.SetActive(true);

            TypeWrite.Start(dialogText, npc.text);
            
            start_timer = true;
            timer = 0;
            Debug.Log("Interact!!!");
        }
        private void Update()
        {
            if (start_timer)
            {
                timer += 1 * Time.deltaTime;
            }
            if(timer > npc.text.Length /2)
            {
                dialogWindow.SetActive(false);
                
            }
        }
    }
}

