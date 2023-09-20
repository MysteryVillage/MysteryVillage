using System;
using System.Collections.Generic;
using Inputs;
using Inventory;
using Mirror;
using Network;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yarn.Unity;
using NetworkManager = Network.NetworkManager;

namespace UI
{
    public class PlayerUi : MonoBehaviour
    {
        [Header("Menu Navigation")] 
        public Transform playUi;
        private PlayerController _playerController;
        public List<GameObject> menuOrder;
        public MenuList menuList;
        private int _menuIndex;
        public GameObject pauseMenu;

        public InputIconMap iconMap;

        [Header("Dialogue")]
        private DialogueManager _dialogue;
        public GameObject activeDialogueText;
        public GameObject passiveDialogueText;

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
                _dialogue = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
                _dialogue.activeDialogueViews[0] = activeDialogueText.GetComponent<LineView>();
                _dialogue.passiveDialogueViews[0] = passiveDialogueText.GetComponent<LineView>();
            }
        }

        public void SwitchTo(int index)
        {
            HideAll();
            menuOrder[index].GetComponent<Menu>().Open();
            _menuIndex = index;
        }

        public void HideAll()
        {
            foreach (var menu in menuOrder)
            {
                menu.GetComponent<Menu>().Close();
            }
        }

        private int FindIndexFor(GameObject menu)
        {
            var i = 0;
            foreach (var item in menuOrder)
            {
                if (item.Equals(menu)) return i;
                i++;
            }

            return -1;
        }

        public void OpenBookButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (!_playerController.hasBook) return;
                var index = FindIndexFor(menuList.book);
                if (index >= 0) Open(index);
            }
        }

        public void OpenMapButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var index = FindIndexFor(menuList.map);
                if (index >= 0) Open(index);
            }
        }

        public void OpenInventoryButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var index = FindIndexFor(menuList.inventory);
                if (index >= 0) Open(index);
            }
        }

        public void OpenQuestButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var index = FindIndexFor(menuList.quests);
                if (index >= 0) Open(index);
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
                // TryMenuSwitch(_menuIndex, true);
                
                if (_menuIndex < menuOrder.Count - 1)
                {
                    SwitchTo(_menuIndex + 1);
                }
            }
        }

        public void MenuSwitchLeft(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // TryMenuSwitch(_menuIndex, false);

                if (_menuIndex > 0)
                {
                    SwitchTo(_menuIndex - 1);
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

        public Vector2 GetScreenPosition(Vector3 position, Image waypointImage, Vector3 offset, Camera cam)
        {
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
            Vector2 pos = cam.WorldToScreenPoint(position + offset);

            // var heading = cam.transform
            
            // Check if the target is behind us, to only show the icon once the target is in front
            if(Vector3.Dot((position - cam.transform.position), cam.transform.forward) < 0)
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

    [Serializable]
    public class MenuList
    {
        public GameObject quests;
        public GameObject inventory;
        public GameObject book;
        public GameObject map;
    }
}
