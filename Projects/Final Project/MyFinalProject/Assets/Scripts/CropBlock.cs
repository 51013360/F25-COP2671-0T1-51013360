using UnityEngine;
using UnityEngine.Tilemaps;

public class CropBlock : MonoBehaviour
{
    [Header("Visuals (assign in prefab)")]
    public SpriteRenderer soilSR;
    public SpriteRenderer cropSR;
    public SpriteRenderer waterSR;

    [Header("Sprites (assign in prefab)")]
    public Sprite plowedSprite;
    public Sprite waterSprite;
    public Sprite cropSprite;

    [Header("Prefabs")]
    public GameObject currentSeedlingPrefab;

    [HideInInspector] public bool isWatered = false;

    private bool isPlowed = false;
    private bool isPlanted = false;
    private Seedling planting;

    private void Awake()
    {
        // Ensure all visuals are correctly reset when the block is created
        if (cropSR != null) cropSR.sprite = null;
        if (waterSR != null) waterSR.sprite = null;

        // Reset all states
        isPlowed = false;
        isPlanted = false;
        isWatered = false;
        planting = null;
    }

    private void OnEnable()
    {
        // Subscribe to the OnNewDay event from TimeManager
        TimeManager.OnNewDay.AddListener(HandleNewDay);
    }

    private void OnDisable()
    {
        // Unsubscribe when the object is disabled to prevent memory leaks
        TimeManager.OnNewDay.RemoveListener(HandleNewDay);
    }

    public void PlowCrop()
    {
        // Can only plow if not already plowed and during the day
        if (!isPlowed && TimeManager.isDay)
        {   
            if (soilSR != null && plowedSprite != null)
                soilSR.sprite = plowedSprite;
            isPlowed = true;
            Debug.Log($"Plowed at {transform.position}");
        }
    }

    public void WaterCrop()
    {
        // Check if the block is plowed and not already watered
        if (!isPlowed) return;
        if (isWatered) return;

        // Update the water sprite to indicate watering
        if (waterSR != null && waterSprite != null)
            waterSR.sprite = waterSprite;

        isWatered = true;
        Debug.Log($"Watered at {transform.position}");
    }

    public void PlantCrop()
    {
        // Can only plant if the block is plowed, watered, and not already planted
        if (!isPlowed || !isWatered || isPlanted) return;

        // Get the current selected prefab from CropSelectorLink
        GameObject prefabToPlant = FindFirstObjectByType<CropSelectorLink>().selectedPrefab;
        if (prefabToPlant == null) return;

        // Instantiate the seedling prefab as a child of the crop sprite renderer
        GameObject go = Instantiate(prefabToPlant, cropSR.transform);
        go.transform.localPosition = Vector3.zero;

        // Set up the Seedling component
        planting = go.GetComponent<Seedling>();
        planting.parentBlock = this;
        isPlanted = true;

        // Update the crop sprite to indicate planting
        if (cropSprite != null)
            cropSR.sprite = cropSprite;

        Debug.Log($"Planted {prefabToPlant.name} at {transform.position}");
    }

    public void HarvestCrop()
    {
        // Can only harvest if there is a planting and it's ready to harvest
        if (planting == null) return;

        // Check if the crop is ready to harvest
        if (planting.readyToHarvest)
        {
            planting.ConvertToYield();
            planting = null;
            isPlanted = false;
            if (cropSR != null) cropSR.sprite = null;
            Debug.Log($"Harvested at {transform.position}");
        }
        else
        {
            Debug.Log("Crop is not ready to harvest.");
        }
    }

    private void HandleNewDay()
    {
        // Reset watering each day
        Invoke("IsWatered", 0.3f);
        if (waterSR != null) waterSR.sprite = null;
    }

    private void IsWatered()
    {
        isWatered = false;
    }
}