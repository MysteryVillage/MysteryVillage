using UnityEngine;
using UnityEngine.UI;
using Items;
using System.Collections.Generic;

public class ItemHistory : MonoBehaviour
{
    public Transform itemListParent;
    public GameObject itemUIPrefab;
    private List<ItemData> collectedItems = new List<ItemData>();

    public void CollectItem(int itemId)
    {
        ItemData item = ItemData.FindById(itemId);
        collectedItems.Add(item);
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Liste leeren
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        // Aufgesammelte Items in Liste
        foreach (ItemData item in collectedItems)
        {
            GameObject itemUI = Instantiate(itemUIPrefab, itemListParent);

            // set Icon
            Image iconImage = itemUI.GetComponentInChildren<Image>();
            iconImage.sprite = item.icon;

            // set Name
            Text nameText = itemUI.GetComponentInChildren<Text>();
            nameText.text = item.displayName;
        }
    }
}
