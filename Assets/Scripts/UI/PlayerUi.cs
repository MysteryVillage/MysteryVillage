using System.Collections.Generic;
using Inputs;
using Inventory;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

        [Header("Waypoints")] 
        public WaypointManager waypointManager;

        // Start is called before the first frame update
        void Start()
        {
            var networkManager = GameObject.Find("NetworkManager");
            networkManager.GetComponent<NetworkManager>().HideLoadingScreen();
            _playerController = transform.parent.GetComponent<PlayerController>();
            if (GameObject.Find("Dialogue") != null)
            {
                _dialogue = GameObject.Find("Dialogue").GetComponent<DialogueRunner>();
                _dialogue.dialogueViews[0] = dialogueText.GetComponent<LineView>();
                _dialogue.dialogueViews[1] = dialogueOptions.GetComponent<OptionsListView>();
            }
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

        public void OpenQuestButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _menuIndex = 3;
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
            var netTransform = transform.parent.GetComponent<NetworkTransformReliable>();
            _playerController.enabled = false;
            netTransform.RpcTeleport(pos.position);
            _playerController.enabled = true;
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

        public Vector2 GetScreenPosition(Transform target, Image waypointImage, Vector3 offset)
        {
            var cam = Camera.main;
            // Giving limits to the icon so it sticks on the screen
            // Below calculations witht the assumption that the icon anchor point is in the middle
            // Minimum X position: half of the icon width
            float minX = waypointImage.GetPixelAdjustedRect().width / 2;
            // Maximum X position: screen width - half of the icon width
            float maxX = Screen.width - minX;

            // Minimum Y position: half of the height
            float minY = waypointImage.GetPixelAdjustedRect().height / 2;
            // Maximum Y position: screen height - half of the icon height
            float maxY = Screen.height - minY;

            // Temporary variable to store the converted position from 3D world point to 2D screen point
            Vector2 pos = cam.WorldToScreenPoint(target.position + offset);

            // var heading = cam.transform
            
            // Check if the target is behind us, to only show the icon once the target is in front
            if(Vector3.Dot((target.position - cam.transform.position), cam.transform.forward) < 0)
            {
                // Check if the target is on the left side of the screen
                if(pos.x < Screen.width / 2)
                {
                    // Place it on the right (Since it's behind the player, it's the opposite)
                    pos.x = maxX;
                }
                else
                {
                    // Place it on the left side
                    pos.x = minX;
                }
            }

            // Limit the X and Y positions
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            return pos;
        }
    }
}
