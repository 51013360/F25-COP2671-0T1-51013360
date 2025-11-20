using UnityEngine;
using UnityEngine.Tilemaps;

public class CropBlock : MonoBehaviour
{
    // Visual references (assign these on the PREFAB)
    public SpriteRenderer soilSR;
    public SpriteRenderer cropSR;
    public SpriteRenderer waterSR;

    // Sprites to assign in inspector (on the prefab)
    [Tooltip("Sprite to use when the tile is plowed")]
    public Sprite plowedSprite;

    [Tooltip("Sprite to show when watered (overlay)")]
    public Sprite waterSprite;

    [Tooltip("Sprite to show when a crop is visible (optional placeholder)")]
    public Sprite cropSprite;

    // Prefab for the seedling (assign in the prefab)
    public GameObject seedlingPrefab;

    // Runtime state (NOT serialized) — new clones will always start with these defaults
    private bool isPlowed = false;
    public bool isWatered = false;
    private bool isPlanted = false;

    // If a seedling is spawned, we store its Seedling component here
    private Seedling planting;

    // --------------------
    // Initialization
    // --------------------
    private void Awake()
    {
        // Ensure runtime flags are initialized (prevents prefab inspector values from leaking)
        isPlowed = false;
        isWatered = false;
        isPlanted = false;
        planting = null;

        // Initialize visuals: hide crop and water overlays by default
        if (cropSR != null)
            cropSR.sprite = null;

        if (waterSR != null)
            waterSR.sprite = null;

        // Optionally, ensure soil SR has some default - we leave whatever the prefab soil sprite is.
        // The plow operation will set it to plowedSprite (so make sure plowedSprite is assigned).
    }

    private void OnEnable()
    {
        TimeManager.OnNewDay.AddListener(HandleNewDay);
    }

    private void OnDisable()
    {
        TimeManager.OnNewDay.RemoveListener(HandleNewDay);
    }

    // --------------------
    // Actions
    // --------------------
    public void PlowCrop()
    {
        if (!isPlowed)
        {
            if (soilSR != null && plowedSprite != null)
                soilSR.sprite = plowedSprite;
            isPlowed = true;
            Debug.Log($"Plowed at {transform.position}");
        }
        else
        {
            Debug.Log("Already plowed");
        }
    }

    public void WaterCrop()
    {
        if (!isPlowed)
        {
            Debug.Log("Not Plowed");
            return;
        }

        if (isWatered)
        {
            Debug.Log("Already watered");
            return;
        }

        if (waterSR != null && waterSprite != null)
            waterSR.sprite = waterSprite;

        isWatered = true;
        Debug.Log($"Watered at {transform.position}");
    }

    public void PlantCrop()
    {
        if (!isPlowed)
        {
            Debug.Log("Can't plant: not plowed");
            return;
        }

        if (!isWatered)
        {
            Debug.Log("Can't plant: not watered");
            return;
        }

        if (isPlanted)
        {
            Debug.Log("Already planted");
            return;
        }

        // Spawn the actual seedling prefab as a child so it has its own component instance
        if (seedlingPrefab != null)
        {
            GameObject go = Instantiate(seedlingPrefab, cropSR.transform);
            go.transform.localPosition = Vector3.zero; // keep it aligned

            planting = go.GetComponent<Seedling>();
            planting.parentBlock = this; 

            // Optionally set a placeholder sprite on crop renderer
            if (cropSR != null && cropSprite != null)
                cropSR.sprite = cropSprite;

            isPlanted = true;
            Debug.Log($"Planted at {transform.position}");
        }
        else
        {
            Debug.LogWarning("No seedlingPrefab assigned on CropBlock prefab.");
        }
    }

    public void HarvestCrop()
    {
        if (planting == null)
        {
            Debug.Log("Nothing to harvest (no planting instance).");
            return;
        }

        if (planting.readyToHarvest)
        {
            planting.ConvertToYield();
            planting = null;
            isPlanted = false;
            // Clear crop visuals if you want
            if (cropSR != null)
                cropSR.sprite = null;

            Debug.Log($"Harvested at {transform.position}");
        }
        else
        {
            Debug.Log("Not ready to harvest yet.");
        }
    }

    // --------------------
    // Time handling
    // --------------------
    private void HandleNewDay()
    {
        // Reset watering each day
        Invoke("IsWatered", 0.3f);

        // Hide water overlay
        if (waterSR != null)
            waterSR.sprite = null;

        // Optionally progress planted seedlings are handled by the Seedling component itself (it listens to TimeManager)
        Debug.Log($"New day: reset water at {transform.position}");
    }

    public void IsWatered()
    {
        isWatered = false;
    }
}
