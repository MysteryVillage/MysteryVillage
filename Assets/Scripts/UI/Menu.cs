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

        [Header("MenuControls")]
        public GameObject optionsScreen;
        public GameObject optionsFirstButton,
        optionsClosedButton;

        public void Open()
        {
            // Allow further events
            onOpen.Invoke();
            
            gameObject.SetActive(true);
            if (startButton != null) EventSystem.current.SetSelectedGameObject(startButton);
        }

        public void Close()
        {
            // Allow further events
            onClose.Invoke();
            
            gameObject.SetActive(false);
        }

        public void OpenOption()
        {
            optionsScreen.SetActive(true);

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new selected object
            EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        }

        public void CloseOption()
        {
            optionsScreen.SetActive(false);

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new selected object
            EventSystem.current.SetSelectedGameObject(optionsClosedButton);
        }
    }
}