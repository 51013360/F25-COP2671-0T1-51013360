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
    public GameObject potatoSeedlingPrefab;
    public GameObject leekSeedlingPrefab;
    public GameObject garlicSeedlingPrefab;
    public GameObject beetSeedlingPrefab;

    [HideInInspector] public bool isWatered = false;

    private bool isPlowed = false;
    private bool isPlanted = false;
    private Seedling planting;

    private void Awake()
    {
        // Ensure initial visuals
        if (cropSR != null) cropSR.sprite = null;
        if (waterSR != null) waterSR.sprite = null;

        isPlowed = false;
        isPlanted = false;
        isWatered = false;
        planting = null;
    }

    private void OnEnable()
    {
        TimeManager.OnNewDay.AddListener(HandleNewDay);
    }

    private void OnDisable()
    {
        TimeManager.OnNewDay.RemoveListener(HandleNewDay);
    }

    public void PlowCrop()
    {
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
        if (!isPlowed) return;
        if (isWatered) return;

        if (waterSR != null && waterSprite != null)
            waterSR.sprite = waterSprite;

        isWatered = true;
        Debug.Log($"Watered at {transform.position}");
    }

    public void PlantCrop()
    {
        if (!isPlowed || !isWatered || isPlanted) return;

        if (currentSeedlingPrefab != null && cropSR != null)
        {
            GameObject go = Instantiate(currentSeedlingPrefab, cropSR.transform);
            go.transform.localPosition = Vector3.zero;

            planting = go.GetComponent<Seedling>();
            planting.parentBlock = this;

            if (cropSprite != null)
                cropSR.sprite = cropSprite;

            isPlanted = true;
            Debug.Log($"Planted at {transform.position}");
        }
    }

    public void HarvestCrop()
    {
        if (planting == null) return;
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
