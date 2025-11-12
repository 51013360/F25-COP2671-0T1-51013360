using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    public FarmingController farmingController;
    public Button hoeButton;
    public Button waterButton;
    public Button plantButton;
    public Button harvestButton;

    [Header("Optional: default seed to plant when Plant button pressed")]
    public SeedPacket defaultSeed;

    void Start()
    {
        hoeButton.onClick.AddListener(() => farmingController.OnHoe());
        waterButton.onClick.AddListener(() => farmingController.OnWater());
        plantButton.onClick.AddListener(() => farmingController.OnPlant(defaultSeed));
        harvestButton.onClick.AddListener(() => farmingController.OnHarvest());
    }
}