using Items;
using UnityEngine;

namespace Inventory
{
    public class InventoryData
    {
        public PlayerInventory Inventory;
        public GameObject _player;
        public ItemSlot[] slots;

        public InventoryData() {}
    
        public InventoryData(int slotNumber, GameObject player)
        {
            slots = new ItemSlot[slotNumber];
            for (int x = 0; x < slotNumber; x++)
            {
                slots[x] = new ItemSlot();
            }
            _player = player;
            Inventory = _player.GetComponent<PlayerInventory>();
        }

        ItemSlot GetItemStack(ItemData item)
        {
            for (int x = 0; x < slots.Length; x++)
            {
                if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
                {
                    return slots[x];
                }
            }
        
            return null;
        }

        ItemSlot GetEmptySlot()
        {
            for (int x = 0; x < slots.Length; x++)
            {
                if (slots[x].item == null)
                {
                    return slots[x];
                }
            }

            return null;
        }

        public void AddItem(int itemId)
        {
            ItemData item = InventoryManager.Instance().availableItems[itemId];
            if (item.canStack)
            {
                ItemSlot slotToStackTo = GetItemStack(item);

                if (slotToStackTo != null)
                {
                    slotToStackTo.quantity++;
                    RefreshInventory();
                    return;
                }
            }

            ItemSlot emptySlot = GetEmptySlot();

            if (emptySlot != null)
            {
                emptySlot.item = item;
                emptySlot.quantity = 1;
                RefreshInventory();
                return;
            }
        
            // @TODO
            // Drop again if inventory is full
            // inventory.ThrowItem(item);
        }

        public void RemoveItemFromIndex(int slotIndex, int amount = 1)
        {
            Debug.Log(this);
        
            var selectedItem = slots[slotIndex];
            selectedItem.quantity = selectedItem.quantity - amount;

            if (selectedItem.quantity == 0)
            {
                selectedItem.item = null;
            }

            Debug.Log(this);
        
            RefreshInventory();
        }

        void RefreshInventory()
        {
            SortInventory();
            if (InventoryManager.Instance().isServer)
            {
                // Debug.Log("Update Ui from Server");
                Inventory.UpdateUi(GetStrippedItemSlots());
            }
        }

        StrippedItemSlot[] GetStrippedItemSlots()
        {
            var strippedSlots = new StrippedItemSlot[slots.Length];
        
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    // Debug.Log("Strip slot: " + slots[i].item.GetId());
                    strippedSlots[i] = new StrippedItemSlot(slots[i].item.GetId(), slots[i].quantity);                
                }
                else
                {
                    // Debug.Log("Strip slot: empty");
                    strippedSlots[i] = new StrippedItemSlot(-1, 0);
                }
            }

            return strippedSlots;
        }

        void SortInventory()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    SwapWithNextUsedSlot(i);
                }
            }
        }

        void SwapWithNextUsedSlot(int index)
        {
            var nextUsedSlotIndex = GetNextUsedSlot(index);
            if (nextUsedSlotIndex == -1)
            {
                return;
            }

            slots[index].Set(slots[nextUsedSlotIndex].item, slots[nextUsedSlotIndex].quantity);
            slots[nextUsedSlotIndex].Clear();
        }

        int GetNextUsedSlot(int index)
        {
            if (slots.Length > index + 2) {
                if (slots[index + 1].item != null)
                {
                    return index + 1;
                }
                return GetNextUsedSlot(index + 1);
            }

            return -1;
        }

        public bool HasItem(ItemData item, int amount)
        {
            foreach (var slot in slots)
            {
                Debug.Log(slot);
                if (slot.item != null) {
                    if (slot.item.Equals(item) && slot.quantity >= amount)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            var text = "Items of player " + _player.name + ": \n";
            for (int i = 0; i < slots.Length; i++)
            {
                text += slots[i] + "\n";
            }

            return text;
        }

        public void TryRemoveItem(ItemData item, int amount)
        {
            if (!HasItem(item, amount))
            {
                return;
            }
            
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                if (slot.item.Equals(item) && slot.quantity >= amount)
                {
                    RemoveItemFromIndex(i, amount);
                    return;
                }
            }
        }
    }
}
