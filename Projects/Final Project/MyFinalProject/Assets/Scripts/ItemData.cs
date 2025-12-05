using UnityEngine;

// This ScriptableObject represents an item in the farm game.
[CreateAssetMenu(menuName = "Farm/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName; // The name of the item
    public Sprite icon;     // The icon to display in the UI for this item
}
