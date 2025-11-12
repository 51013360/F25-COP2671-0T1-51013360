using System.Collections;
using UnityEngine;

public class CropBlock : MonoBehaviour
{
    public enum CropState { Empty, Plowed, Planted, Growing, ReadyToHarvest }

    [Header("Inspector references")]
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public Vector2Int GridLocation; // tile coords
    [HideInInspector] public CropState State = CropState.Empty;
    [HideInInspector] public SeedPacket plantedSeed;
    [HideInInspector] public int currentStage = 0; // 0..3
    [HideInInspector] public bool watered = false;

    Coroutine growthRoutine;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Called when soil is ploughed
    public void TillSoil()
    {
        if (State == CropState.Empty)
        {
            State = CropState.Plowed;
            spriteRenderer.sprite = null; // show ploughed soil sprite if you have one
        }
    }

    public void WaterSoil()
    {
        if (State == CropState.Plowed || State == CropState.Planted || State == CropState.Growing)
        {
            watered = true;
        }
    }

    public void PlantSeed(SeedPacket seed)
    {
        if (seed == null) return;
        if (State == CropState.Plowed)
        {
            plantedSeed = seed;
            State = CropState.Planted;
            currentStage = 0;
            UpdateSprite();
            // begin growth coroutine
            if (growthRoutine != null) StopCoroutine(growthRoutine);
            growthRoutine = StartCoroutine(GrowthRoutine());
        }
    }

    IEnumerator GrowthRoutine()
    {
        State = CropState.Growing;
        while (currentStage < Mathf.Min(3, plantedSeed.growthSprites.Length - 1))
        {
            // Only advance if watered; otherwise wait and try again each second
            float timer = 0f;
            while (timer < plantedSeed.secondsPerStage)
            {
                // if watered, accelerate / allow timer to progress
                if (watered) timer += Time.deltaTime;
                else timer += Time.deltaTime * 0.2f; // slow dry growth (adjust or require watering)
                yield return null;
            }

            currentStage++;
            watered = false; // consume watering
            UpdateSprite();
            yield return null;
        }

        // reached final stage
        State = CropState.ReadyToHarvest;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (plantedSeed == null)
        {
            spriteRenderer.sprite = null;
            return;
        }

        int idx = Mathf.Clamp(currentStage, 0, plantedSeed.growthSprites.Length - 1);
        spriteRenderer.sprite = plantedSeed.growthSprites[idx];
    }

    public void Harvest()
    {
        if (State == CropState.ReadyToHarvest)
        {
            // spawn harvestable if provided
            if (plantedSeed.harvestablePrefab != null)
            {
                Instantiate(plantedSeed.harvestablePrefab, transform.position, Quaternion.identity);
            }
            // reset
            plantedSeed = null;
            currentStage = 0;
            State = CropState.Empty;
            spriteRenderer.sprite = null;
        }
    }

    // Optional: helper to force-stop growth (used when removing)
    public void ClearBlock()
    {
        if (growthRoutine != null) StopCoroutine(growthRoutine);
        plantedSeed = null;
        currentStage = 0;
        watered = false;
        State = CropState.Empty;
        spriteRenderer.sprite = null;
    }
}
