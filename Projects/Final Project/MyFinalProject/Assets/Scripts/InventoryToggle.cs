using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
