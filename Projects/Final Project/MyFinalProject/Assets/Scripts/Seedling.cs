using UnityEngine;
using UnityEngine.U2D;
using static Seedling;

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
    //public CropYield cropYield;
    public bool readyToHarvest;

    private GrowthStage currentStage = GrowthStage.Seed;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool nextDay;
    
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

    private void Update()
    {
        if (nextDay == false || currentStage > GrowthStage.Mature) return;
        nextDay = false;

        currentStage++;
        spriteRenderer.sprite = growthSprites[(int)currentStage];

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
        //var yield = Instantiate(cropYield, transform);
        //yield.transform.SetParent(null);

        Destroy(gameObject);
    }

    private void HandleNewDay()
    {
        nextDay = true;
    }
}
