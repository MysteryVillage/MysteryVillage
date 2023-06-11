using System;
using Items;
using Mirror;
using Network;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
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
            
            // If is last of its kind
            if (slots[_selectedItemIndex].quantity < 2)
            {
                ClearSelectedItemWindow();
            }
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
                    
                    var selected = EventSystem.current.currentSelectedGameObject;

                    if (selected != uiSlots[x].gameObject || selected == null) continue;
                    
                    var lastSlot = GetLastUsedSlot();
                    Debug.Log(lastSlot);
                    if (lastSlot != null)
                    {
                        EventSystem.current.SetSelectedGameObject(lastSlot.gameObject);
                    }
                }
            }
        }

        ItemSlotUI GetLastUsedSlot()
        {
            for (int i = uiSlots.Length; i >= 0; i--)
            {
                if (slots[i - 1].itemId != -1)
                {
                    return uiSlots[i - 1];
                }
            }
            return null;
        }

        public void SelectItem(int index)
        {
            // Debug.Log("Select item: " + index);
            // Debug.Log("Select item: " + slots[index].itemId);
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

        public void Set(ItemData item)
        {
            if (item == null)
            {
                Clear();
                return;
            }
            
            Set(item, 1);
        }

        public void Set(ItemData item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
            itemId = item.GetId();
        }

        public void Clear()
        {
            item = null;
            itemId = -1;
            quantity = 0;
        }
    }
}