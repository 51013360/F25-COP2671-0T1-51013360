using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Dictionary to hold item names and their quantities
    public Dictionary<string, int> items = new Dictionary<string, int>();
    public System.Action OnInventoryChanged;

    public void AddItem(ItemData item)
    {
        // Check if the item already exists in the inventory
        if (items.ContainsKey(item.itemName))
            items[item.itemName]++;
        else
            items[item.itemName] = 1;

        // Notify listeners that the inventory has changed
        OnInventoryChanged?.Invoke();
    }
}