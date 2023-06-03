using System;
using Items;
using Mirror;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class PlayerInventory : NetworkBehaviour
    {
        public ItemSlotUI[] uiSlots;
        public StrippedItemSlot[] slots;
    
        public GameObject inventoryWindow;
        public Transform dropPosition;

        [Header("Selected Item")] 
        private ItemSlot _selectedItem;
        private int _selectedItemIndex;
        public TextMeshProUGUI selectedItemName;
        public TextMeshProUGUI selectedItemDescription;
        public GameObject dropButton;

        // components
        private PlayerController _controller;


        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Start()
        {
            inventoryWindow.SetActive(false);
        
            // init the slots
            for (int x = 0; x < uiSlots.Length; x++)
            {
                uiSlots[x].index = x;
                uiSlots[x].Clear();
            }
        
            ClearSelectedItemWindow();
        }

        public void OnInventoryButton(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            if (inventoryWindow.activeInHierarchy)
            {
                inventoryWindow.SetActive(false);
                _controller.SetActionMap("Player");
            }
            else
            {
                inventoryWindow.SetActive(true);
                ClearSelectedItemWindow();
                _controller.SetActionMap("UI");
            }
        }

        public void CollectItem(int itemId)
        {
            print("Is server2: " + isServer);
            InventoryManager.Instance().AddItemToInventory(itemId, GetComponent<NetworkIdentity>().netId);
        }

        public void OnDropItemButton()
        {
            if (_selectedItem == null) return;
            print("Is server: " + isServer + "(OnDropItemButton)");
            DropItemCmd(_selectedItemIndex, _selectedItem.item.GetId());
            ClearSelectedItemWindow();
        }

        [Command]
        public void DropItemCmd(int itemIndex, int itemId)
        {
            print("Is server: " + isServer + "(DropItem)");
            InventoryManager.Instance().RemoveItemFromInventoryByIndex(itemIndex, GetComponent<NetworkIdentity>().netId);
            ItemManager.Instance().Spawn(itemId, dropPosition.position);
        }

        [ClientRpc]
        public void UpdateUi(StrippedItemSlot[] strippedSlots)
        {
            Debug.Log("update Ui");
            slots = strippedSlots;
            for (int x = 0; x < uiSlots.Length; x++)
            {
                var slot = strippedSlots[x];
                if (slot.itemId != -1)
                {
                    uiSlots[x].Set(slot.Enrich());
                }
                else
                {
                    uiSlots[x].Clear();
                }
            }
        }

        public void SelectItem(int index)
        {
            if (slots[index].itemId == -1)
            {
                return;
            }

            _selectedItem = slots[index].Enrich();
            _selectedItemIndex = index;

            selectedItemName.text = _selectedItem.item.displayName;
            selectedItemDescription.text = _selectedItem.item.description;
        
            dropButton.SetActive(true);
        }

        public void ClearSelectedItemWindow()
        {
            // clear the text elements
            _selectedItem = null;
            selectedItemName.text = string.Empty;
            selectedItemDescription.text = string.Empty;
        
            // disable buttons
            dropButton.SetActive(false);
        }

        public void Close()
        {
            inventoryWindow.SetActive(false);
            _controller.SetActionMap("Player");
        }
    }

    [Serializable]
    public class ItemSlot
    {
        public ItemData item;
        public int itemId;
        public int quantity;
        public override string ToString()
        {
            if (!item)
            {
                return "Empty";
            }
            return item.displayName + "(x" + quantity + ")";
        }
    }
}