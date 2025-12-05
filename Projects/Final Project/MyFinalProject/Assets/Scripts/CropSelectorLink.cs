using UnityEngine;
using TMPro;

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
        string cropName = dropdown.options[index].text;

        switch (cropName)
        {
            case "Potato": selectedPrefab = potatoPrefab; break;
            case "Leek": selectedPrefab = leekPrefab; break;
            case "Garlic": selectedPrefab = garlicPrefab; break;
            case "Beet": selectedPrefab = beetPrefab; break;
            default:
                Debug.LogWarning("Unknown crop selected: " + cropName);
                selectedPrefab = null;
                break;
        }

        Debug.Log("Selected seedling prefab: " + selectedPrefab);
    }
}
