using System;
using Mirror;
using Network;
using StarterAssets;
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
        private ItemSlot selectedItem;
        private int selectedItemIndex;
        public TextMeshProUGUI selectedItemName;
        public TextMeshProUGUI selectedItemDescription;
        public GameObject dropButton;

        // components
        private ThirdPersonController controller;


        private void Awake()
        {
            controller = GetComponent<ThirdPersonController>();
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
                controller.ToggleCursor(false);
            }
            else
            {
                inventoryWindow.SetActive(true);
                ClearSelectedItemWindow();
                controller.ToggleCursor(true);
            }
        }

        public void CollectItem(int itemId)
        {
            print("Is server2: " + isServer);
            InventoryManager.Instance().AddItemToInventory(itemId, GetComponent<NetworkIdentity>().netId);
        }

        public void OnDropItemButton()
        {
            print("Is server: " + isServer + "(OnDropItemButton)");
            DropItemCmd(selectedItemIndex, selectedItem.item.GetId());
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

            selectedItem = slots[index].Enrich();
            selectedItemIndex = index;

            selectedItemName.text = selectedItem.item.displayName;
            selectedItemDescription.text = selectedItem.item.description;
        
            dropButton.SetActive(true);
        }

        public void ClearSelectedItemWindow()
        {
            // clear the text elements
            selectedItem = null;
            selectedItemName.text = string.Empty;
            selectedItemDescription.text = string.Empty;
        
            // disable buttons
            dropButton.SetActive(false);
        }

        ItemSlot[] Slots()
        {
            return InventoryManager.Instance().Get(GetComponent<NetworkIdentity>().netId).slots;
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