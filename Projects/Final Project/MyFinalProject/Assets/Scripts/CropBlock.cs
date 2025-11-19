using UnityEngine;

public class CropBlock : MonoBehaviour
{
    public SpriteRenderer soilSR;
    public SpriteRenderer cropSR;
    public SpriteRenderer waterSR;
    public Seedling currentSeedPrefab;

    private Sprite soilSprite;
    private Sprite waterSprite;
    private Sprite cropSprite;

    private bool isPlowed;
    private bool isWatered;
    private Seedling planting;

    public void Awake()
    {
        soilSprite = soilSR.sprite;
        waterSprite = waterSR.sprite;
        cropSprite = cropSR.sprite;

        soilSR.sprite = null;
        waterSR.sprite = null;
        cropSR.sprite = null;
    }

    public void PlowCrop()
    {
        if (isPlowed == false)
        {
            soilSR.sprite = soilSprite;
            isPlowed = true;
        }
    }

    public void WaterCrop()
    {
        if (isPlowed == false) return;
        if (isWatered) return;

        waterSR.sprite = waterSprite;
        isWatered = true;
    }

    public void PlantCrop()
    {
        cropSR.sprite = cropSprite;
        planting = Instantiate(currentSeedPrefab, cropSR.transform);
    }

    public void HarvestCrop()
    {
        if (planting.readyToHarvest)
        {
            planting.ConvertToYield();
        }
    }
}
