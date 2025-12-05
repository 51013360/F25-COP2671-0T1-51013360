using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotParent;

    private void OnEnable()
    {
        // Subscribe to the event
        inventory.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to prevent memory leaks
        inventory.OnInventoryChanged -= Refresh;
    }

    void Refresh()
    {
        // Clear existing slots
        foreach (Transform t in slotParent)
            Destroy(t.gameObject);

        // Create new slots
        foreach (var pair in inventory.items)
        {
            // slot
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = pair.Value.ToString();

            // icon
            ItemData data = Resources.Load<ItemData>("Items/" + pair.Key);
            slot.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = data.icon;
        }
    }
}