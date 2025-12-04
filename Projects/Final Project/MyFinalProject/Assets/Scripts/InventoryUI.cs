using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public Transform slotGrid;

    private InventorySlotUI[] uiSlots;
    private bool isOpen = false;

    void Awake()
    {
        Instance = this;

        // Create 8 UI slots dynamically
        uiSlots = new InventorySlotUI[8];

        for (int i = 0; i < 8; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotGrid);
            uiSlots[i] = slotObj.GetComponent<InventorySlotUI>();
            uiSlots[i].Clear();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
            RefreshUI();
    }

    public void RefreshUI()
    {
        var items = InventorySystem.Instance.slots;

        for (int i = 0; i < 8; i++)
        {
            if (items[i] != null)
                uiSlots[i].SetItem(items[i].icon, items[i].count);
            else
                uiSlots[i].Clear();
        }
    }
}
