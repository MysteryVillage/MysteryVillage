using System;
using Items;
using UnityEngine;

namespace UI
{
    public class ItemPickupController : MonoBehaviour
    {
        public GameObject itemPickupNotice;

        private void Start()
        {
            // Clear list
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void AddItemNotice(int itemId)
        {
            // Get item data
            var item = ItemData.FindById(itemId);
            
            // Create UI object
            var instance = Instantiate(itemPickupNotice, gameObject.transform, true);
            var notice = instance.GetComponent<ItemPickupNotice>();
            
            // Set item data for UI object
            notice.text.text = String.Format("{0} aufgesammelt", item.displayName);
            notice.icon.sprite = item.icon;
        }
    }
}