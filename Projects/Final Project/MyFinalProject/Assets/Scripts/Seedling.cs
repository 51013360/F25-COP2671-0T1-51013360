using UnityEngine;

public class Seedling : MonoBehaviour
{
    public enum GrowthStage
    {
        None = -1,
        Seed = 0,
        Sprout = 1,
        Young = 2,
        Mature = 3,
    }

    public Sprite[] growthSprites;
    public Sprite seedCover;
    public bool readyToHarvest;

    private GrowthStage currentStage = GrowthStage.Seed;
    private SpriteRenderer spriteRenderer;

    // Assigned by CropBlock when planting
    [HideInInspector]public CropBlock parentBlock;

    private bool nextDay;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Save original sprite as seed cover
        seedCover = spriteRenderer.sprite;

        // Seedlings start invisible (seed under dirt)
        spriteRenderer.sprite = null;
    }

    private void OnEnable()
    {
        TimeManager.OnNewDay.AddListener(HandleNewDay);
    }

    private void OnDisable()
    {
        TimeManager.OnNewDay.RemoveListener(HandleNewDay);
    }

    private void Update()
    {
        if (!nextDay) return;
        nextDay = false;

        // Only grow if watered
        if (parentBlock == null || !parentBlock.isWatered)
            return;

        if (currentStage >= GrowthStage.Mature)
            return;

        // Grow
        currentStage++;
        spriteRenderer.sprite = growthSprites[(int)currentStage];

        // After growing, consume water for the next day
        parentBlock.isWatered = false;
        if (parentBlock.waterSR != null)
            parentBlock.waterSR.sprite = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        readyToHarvest = currentStage == GrowthStage.Mature;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        readyToHarvest = false;
    }

    public void ConvertToYield()
    {
        Destroy(gameObject);
    }

    private void HandleNewDay()
    {
        nextDay = true;
    }
}