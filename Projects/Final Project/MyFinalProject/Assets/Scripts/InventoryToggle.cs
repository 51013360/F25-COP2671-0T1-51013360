using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;

    private void Update()
    {
        // Toggle inventory panel visibility when 'I' key is pressed
        if (Keyboard.current.iKey.wasPressedThisFrame)
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
