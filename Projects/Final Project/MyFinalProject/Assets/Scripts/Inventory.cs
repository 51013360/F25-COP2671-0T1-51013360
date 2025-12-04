using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> items = new Dictionary<string, int>();
    public System.Action OnInventoryChanged;

    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item.itemName))
            items[item.itemName]++;
        else
            items[item.itemName] = 1;

        OnInventoryChanged?.Invoke();
    }
}
