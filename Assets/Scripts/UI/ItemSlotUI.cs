using System;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ItemSlotUI : MonoBehaviour, ISelectHandler
    {
        public Button button;
        public Image icon;
        public TextMeshProUGUI quantityText;
        private ItemSlot curSlot;
        private Outline outline;

        public int index;
        public bool equipped;

        private void Awake()
        {
            outline = GetComponent<Outline>();
        }

        private void OnEnable()
        {
            outline.enabled = equipped;
        }

        public void Set(ItemSlot slot)
        {
            curSlot = slot;

            icon.gameObject.SetActive(true);
            icon.sprite = slot.item.icon;
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : String.Empty;

            if (outline != null)
            {
                outline.enabled = equipped;
            }

            button.interactable = true;
        }

        public void Set(StrippedItemSlot slot)
        {
            Set(slot.Enrich());
        }

        public void Clear()
        {
            curSlot = null;

            icon.gameObject.SetActive(false);
            quantityText.text = String.Empty;

            button.interactable = false;
        }

        public void OnButtonClick()
        {
            Select();
        }

        public void OnSelect(BaseEventData eventData)
        {
            Select();
        }

        private void Select()
        {
            transform.GetComponentInParent<PlayerInventory>().SelectItem(index);
        }

    }
}