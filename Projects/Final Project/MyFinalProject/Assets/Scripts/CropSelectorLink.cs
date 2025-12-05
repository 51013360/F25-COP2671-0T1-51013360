using UnityEngine;
using TMPro;

// This script links a TMP_Dropdown UI to the prefabs of crops.
// It determines which crop prefab is currently selected for planting.
public class CropSelectorLink : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    [Header("Assign CropBlock Prefabs")]
    public GameObject potatoPrefab;
    public GameObject leekPrefab;
    public GameObject garlicPrefab;
    public GameObject beetPrefab;

    [HideInInspector] public GameObject selectedPrefab;

    private void Start()
    {
        if (dropdown == null)
        {
            // Error if dropdown is not assigned in the inspector
            Debug.LogError("Dropdown not assigned!");
            return;
        }

        // Set default selection
        UpdateSelectedPrefab(dropdown.value);

        // Listen to changes
        dropdown.onValueChanged.AddListener(UpdateSelectedPrefab);
    }

    private void UpdateSelectedPrefab(int index)
    {
        // Get the text of the selected dropdown option
        string cropName = dropdown.options[index].text;

        // Assign the corresponding prefab based on the selected crop
        switch (cropName)
        {
            case "Potato": selectedPrefab = potatoPrefab; break;
            case "Leek": selectedPrefab = leekPrefab; break;
            case "Garlic": selectedPrefab = garlicPrefab; break;
            case "Beet": selectedPrefab = beetPrefab; break;
            default:
                // If the option doesn't match any known crop, log a warning
                Debug.LogWarning("Unknown crop selected: " + cropName);
                selectedPrefab = null;
                break;
        }

        // Log the currently selected prefab for debugging
        Debug.Log("Selected seedling prefab: " + selectedPrefab);
    }
}
