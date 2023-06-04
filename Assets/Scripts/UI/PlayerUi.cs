using System.Collections.Generic;
using Inputs;
using Inventory;
using Mirror;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;
using NetworkManager = Network.NetworkManager;

namespace UI
{
    public class PlayerUi : MonoBehaviour
    {
        [Header("Menu")] 
        public Transform playUi;
        private PlayerController _playerController;
        public List<Transform> menuList;
        private int _menuIndex = 0;
        private bool _menuOpen = false;
        public GameObject pauseMenu;

        public InputIconMap iconMap;

        [Header("Dialogue")]
        private DialogueRunner _dialogue;
        public GameObject dialogueText;
        public GameObject dialogueOptions;

        // Start is called before the first frame update
        void Start()
        {
            _playerController = transform.parent.GetComponent<PlayerController>();
            _dialogue = GameObject.Find("Dialogue").GetComponent<DialogueRunner>();
            _dialogue.dialogueViews[0] = dialogueText.GetComponent<LineView>();
            _dialogue.dialogueViews[1] = dialogueOptions.GetComponent<OptionsListView>();
        }

        public void SwitchTo(int index)
        {
            HideAll();
            menuList[index].gameObject.SetActive(true);
        }

        public void HideAll()
        {
            for (int i = 0; i < menuList.Count; i++)
            {
                menuList[i].gameObject.SetActive(false);
            }
        }

        public void OpenBookButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _menuIndex = 0;
                Open(_menuIndex);
            }
        }

        public void OpenMapButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _menuIndex = 1;
                Open(_menuIndex);
            }
        }

        public void OpenInventoryButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _menuIndex = 2;
                Open(_menuIndex);
                transform.parent.GetComponent<PlayerInventory>().ClearSelectedItemWindow();
            }
        }

        public void Open(int index)
        {
            _menuOpen = true;
            menuList[index].gameObject.SetActive(true);
            playUi.gameObject.SetActive(false);
            _playerController.SetActionMap("UI");
        }

        public void MenuSwitchRight(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (_menuIndex < menuList.Count - 1)
                {
                    _menuIndex++;
                    SwitchTo(_menuIndex);
                }
            }
        }

        public void MenuSwitchLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (_menuIndex > 0)
                {
                    _menuIndex--;
                    SwitchTo(_menuIndex);
                }
            }
        }

        public void CloseUI(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                HideUIElements();
            }
        }

        void HideUIElements()
        {
            _menuOpen = false;
            playUi.gameObject.SetActive(true);
            HideAll();
            _playerController.SetActionMap("Player");
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started && _menuIndex == 2)
            {
                transform.parent.GetComponent<PlayerInventory>().OnDropItemButton();
            }
        }

        public void AdvanceText(InputAction.CallbackContext context)
        {
            if (context.started) dialogueText.GetComponent<LineView>().UserRequestedViewAdvancement();
        }

        public void PauseButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (pauseMenu.activeInHierarchy)
                {
                    ClosePauseMenu();
                }
                else
                {
                    _playerController.SetActionMap("Pause");
                    pauseMenu.SetActive(true);
                }
            }
        }

        public void ClosePauseMenu()
        {
            _playerController.SetActionMap("Player");
            pauseMenu.SetActive(false);
        }

        public void DisconnectButton()
        {
            var manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            if (NetworkServer.active)
            {
                manager.StopHost();
            }

            if (NetworkClient.active)
            {
                manager.StopClient();
            }
        }

        public void ResetButton()
        {
            var pos = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().GetStartPosition();
            transform.parent.GetComponent<NetworkTransformReliable>().RpcTeleport(pos.position);
            ClosePauseMenu();
        }

        public void InputChangeButton()
        {
            // ..
            var input = transform.parent.GetComponent<PlayerInput>();
            Debug.Log(input.currentControlScheme);
        }
    }
}
