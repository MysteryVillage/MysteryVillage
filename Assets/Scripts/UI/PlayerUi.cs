using System.Collections.Generic;
using Inputs;
using Inventory;
using Mirror;
using Player;
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
        private int _menuIndex;
        public GameObject pauseMenu;

        public InputIconMap iconMap;

        [Header("Dialogue")]
        private DialogueRunner _dialogue;
        public GameObject dialogueText;
        public GameObject dialogueOptions;

        [Header("Items")] 
        public ItemPickupController itemPickupController;

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
            menuList[index].GetComponent<Menu>().Open();
        }

        public void HideAll()
        {
            foreach (var menu in menuList)
            {
                menu.GetComponent<Menu>().Close();
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
            }
        }

        public void Open(int index)
        {
            SwitchTo(index);
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
                    pauseMenu.GetComponent<Menu>().Open();
                }
            }
        }

        public void ClosePauseMenu()
        {
            _playerController.SetActionMap("Player");
            pauseMenu.GetComponent<Menu>().Close();
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
            var input = transform.parent.GetComponent<PlayerInput>();
            if (input.currentControlScheme == "Gamepad")
            {
                input.SwitchCurrentControlScheme("KeyboardMouse");
                input.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
            }
            else
            {
                if (Gamepad.current != null) input.SwitchCurrentControlScheme("Gamepad", Gamepad.current);
            }
        }

        public void PickupNotification(int itemId)
        {
            itemPickupController.AddItemNotice(itemId);
        }
    }
}
