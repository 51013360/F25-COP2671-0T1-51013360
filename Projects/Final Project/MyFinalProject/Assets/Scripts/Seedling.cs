using UnityEngine;

public class Seedling : MonoBehaviour
{
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        seedCover = spriteRenderer.sprite;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        readyToHarvest = currentStage == GrowthStage.Mature;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        readyToHarvest = false;
    }
    private void Update()
    {
        if (!nextDay) return;
        nextDay = false;

        if (parentBlock == null || !parentBlock.isWatered) return;
        if (currentStage >= GrowthStage.Mature) return;

        currentStage++;
        spriteRenderer.sprite = growthSprites[(int)currentStage];

        // Consume water after growth
        parentBlock.isWatered = false;
        if (parentBlock.waterSR != null) parentBlock.waterSR.sprite = null;
    }

    public void ConvertToYield()
    {
        Inventory inv = FindAnyObjectByType<Inventory>();

        ItemData data = Resources.Load<ItemData>("Items/" + seedName);

        if (inv != null && data != null)
            inv.AddItem(data);

        Destroy(transform.root.gameObject);
    }

    private void HandleNewDay()
    {
        nextDay = true;
    }
}
