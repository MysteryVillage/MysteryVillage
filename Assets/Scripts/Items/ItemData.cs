using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] 
    public bool canStack;
    public int maxStackAmount;

    public int GetId()
    {
        return ServerInventory.Instance().availableItems.FindIndex(x => x.displayName == displayName);
    }

    public static ItemData FindById(int itemId)
    {
        return ServerInventory.Instance().availableItems[itemId];
    }

    public static int FindId(ItemData item)
    {
        return ServerInventory.Instance().availableItems.FindIndex(x => x.displayName == item.displayName);
    }

    public override string ToString()
    {
        return displayName;
    }
}