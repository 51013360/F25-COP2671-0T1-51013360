using UnityEngine;

public class Seedling : MonoBehaviour
{
    // Enum to represent growth stages
    public enum GrowthStage { None = -1, Seed = 0, Sprout = 1, Young = 2, Mature = 3 }
    public Sprite[] growthSprites;
    public Sprite seedCover;
    public bool readyToHarvest;
    public string seedName;
    public ScriptableObject itemData;

    [HideInInspector] public CropBlock parentBlock;

    private GrowthStage currentStage = GrowthStage.Seed;
    private SpriteRenderer spriteRenderer;
    private bool nextDay = false;

    private void Awake()
    {
        // Initialize components and set initial sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        seedCover = spriteRenderer.sprite;
        spriteRenderer.sprite = null;
    }

    private void OnEnable()
    {
        // Subscribe to the new day event
        TimeManager.OnNewDay.AddListener(HandleNewDay);
    }

    private void OnDisable()
    {
        // Unsubscribe from the new day event
        TimeManager.OnNewDay.RemoveListener(HandleNewDay);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //  Check if the player is in range and the plant is mature
        readyToHarvest = currentStage == GrowthStage.Mature;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset harvest readiness when the player leaves the range
        readyToHarvest = false;
    }
    private void Update()
    {
        // Handle growth at the start of a new day
        if (!nextDay) return;
        nextDay = false;

        // Check if the plant can grow
        if (parentBlock == null || !parentBlock.isWatered) return;
        if (currentStage >= GrowthStage.Mature) return;

        // Advance growth stage
        currentStage++;
        spriteRenderer.sprite = growthSprites[(int)currentStage];

        // Consume water after growth
        parentBlock.isWatered = false;
        if (parentBlock.waterSR != null) parentBlock.waterSR.sprite = null;
    }

    public void ConvertToYield()
    {
        // Add the harvested item to the player's inventory
        Inventory inv = FindFirstObjectByType<Inventory>();

        //  Load the item data for the seed
        ItemData data = Resources.Load<ItemData>("Items/" + seedName);

        // Add the item to the inventory
        if (inv != null && data != null)
        {
            inv.AddItem(data);
        }
        else
        {
            Debug.LogWarning("Inventory or ItemData not found for seed: " + seedName);
        }

        //  Destroy the seedling game object
        Destroy(transform.root.gameObject);
    }

    private void HandleNewDay()
    {
        // Flag to grow the plant on the next update
        nextDay = true;
    }
}