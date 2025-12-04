using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [System.Serializable]
    public class InventoryItem
    {
        public string id;
        public Sprite icon;
        public int count;
    }

    public InventoryItem[] slots = new InventoryItem[8];

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(string id, Sprite icon, int amount)
    {
        // Find existing slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].id == id)
            {
                slots[i].count += amount;
                InventoryUI.Instance.RefreshUI();
                return;
            }
        }

        // Find empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new InventoryItem { id = id, icon = icon, count = amount };
                InventoryUI.Instance.RefreshUI();
                return;
            }
        }
    }
}