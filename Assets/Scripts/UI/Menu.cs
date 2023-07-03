using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using EventSystem = Events.EventSystem;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public GameObject startButton;

        public UnityEvent onOpen;
        public UnityEvent onClose;

        public void Open()
        {
            gameObject.SetActive(true);
            if (startButton != null) EventSystem.current.SetSelectedGameObject(startButton);

            // Allow further events
            onOpen.Invoke();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            
            // Allow further events
            onClose.Invoke();
        }
    }
}