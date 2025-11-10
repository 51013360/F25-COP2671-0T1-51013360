using UnityEngine;

public class CropBlock : MonoBehaviour
{
    // Enum representing the different states a crop tile can be in
    private enum CropState
    {
        Empty,
        Plowed,
        Planted,
        ReadyToHarvest
    }

    // Public property to identify the crop's location on the grid
    public Vector2Int Location { get; protected set; }

    // References to visual components for soil and crop rendering
    [Header("Tilled Soil")]
    [SerializeField] private SpriteRenderer _plowedSoilSR;
    [SerializeField] Sprite _plowedSoilIcon;

    [Header("Watered Soil")]
    [SerializeField] private SpriteRenderer _wateredSoilSR;
    [SerializeField] Sprite _wateredSoilIcon;

    [Header("Crop Soil")]
    [SerializeField] private SpriteRenderer _cropSR;

    // Internal state tracking for crop growth and type
    private SeedPacket.GrowthStage _currentStage;
    private SeedPacket _currentSeedPacket;

    // Flags for crop conditions and interaction control
    private bool _isWatered = false;
    private bool _isWild = false;
    private bool _preventUse = false;

    // Metadata for crop identification
    private string _cropName = string.Empty;
    private string _tilemapName = string.Empty;

    // References to external systems
    //private Validator _validator;
    private CropManager _cropController;
    private ParticleSystem _particleSystem;

    // Current state of the crop tile
    private CropState _currentState = CropState.Empty;

    // Initialization logic for component references
    private void Start()
    {
        //_validator = GetComponentInChildren<Validator>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particleSystem.Stop(); // Ensure particles are off initially
    }

    // Initializes the crop block with location and controller reference
    public void Initialize(string tilemapName, Vector2Int location, CropManager cropController)
    {
        Location = location;
        _tilemapName = tilemapName;
        _cropController = cropController;
        _currentState = CropState.Empty;

        name = FormatName(); // Set object name for debugging
        //DayNightController.Events.OnNewDay.AddListener(NextDay); // Subscribe to daily event
    }

    // Prevents further interaction with this tile
    public void PreventUse() => _preventUse = true;

    // Transitions soil to plowed state
    public void PlowSoil()
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState != CropState.Empty) return;
        if (_preventUse) return;

        _currentState = CropState.Plowed;
        _plowedSoilSR.sprite = _plowedSoilIcon;
    }

    // Waters the soil if conditions are met
    public void WaterSoil()
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState == CropState.Empty) return;
        if (_preventUse) return;
        if (_isWatered) return;

        _wateredSoilSR.sprite = _wateredSoilIcon;
        _isWatered = true;
    }

    // Plants a seed if the soil is plowed
    public void PlantSeed(SeedPacket seedPacket)
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState == CropState.Planted) return;
        if (_currentState != CropState.Plowed) return;

        CreateCrop(seedPacket);
        UpdateCropImage();
    }

    // Adds a wild crop (e.g., spawned naturally)
    public void AddWildCrop(SeedPacket seedPacket)
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState == CropState.Planted) return;

        _isWild = true;
        CreateCrop(seedPacket);
        UpdateCropImage();
    }

    // Instantiates the crop and sets initial growth stage
    private void CreateCrop(SeedPacket seedPacket)
    {
        _currentSeedPacket = Instantiate(seedPacket);
        _currentStage = SeedPacket.GrowthStage.Seed;
        _currentState = CropState.Planted;

        _cropName = _currentSeedPacket.CropName;
        name = FormatName();

        _cropController.AddToPlantedCrops(this);
    }

    // Advances crop growth if watered or wild
    private void GrowPlants()
    {
        if (_currentState != CropState.Planted) return;
        if (!_isWatered && !_isWild) return;

        ++_currentStage;
        if (_currentStage >= SeedPacket.GrowthStage.Mature)
        {
            _particleSystem.Play();
            _currentState = CropState.ReadyToHarvest;
        }
        UpdateCropImage();
    }

    // Harvests the crop if it's mature
    public void HarvestPlants()
    {
        if (_currentState != CropState.ReadyToHarvest) return;

        PickCrop();
        ResetSoil();
    }

    // Instantiates the harvested crop prefab
    private void PickCrop()
    {
        _particleSystem.Stop();

        var crop = Instantiate(_currentSeedPacket.HarvestPrefab, transform);
        crop.transform.SetParent(null); // Detach from tile

        _cropController.RemoveFromPlantedCrops(Location);
    }

    // Updates the crop's visual based on growth stage
    private void UpdateCropImage()
    {
        _cropSR.sprite = _currentSeedPacket.GetIconForStage(_currentStage);
    }

    // Resets the tile to its initial empty state
    private void ResetSoil()
    {
        _currentState = CropState.Empty;
        _isWatered = false;
        _isWild = false;

        _plowedSoilSR.sprite = null;
        _wateredSoilSR.sprite = null;
        _cropSR.sprite = null;
        _cropName = string.Empty;

        name = FormatName();
    }

    // Enables the validator for interaction checks
    //public void TurnValidatorOn()
    //{
    //    if (_validator == null) return;
    //    if (_isWild && _currentStage == SeedPacket.GrowthStage.Mature)
    //    {
    //        _validator.TurnValidatorOn();
    //        return;
    //    }

    //    if (_preventUse) return;
    //    _validator.TurnValidatorOn();
    //}

    //// Disables the validator
    //public void TurnValidatorOff()
    //{
    //    if (_validator == null) return;
    //    _validator.TurnValidatorOff();
    //}

    // Called daily to grow crops and reset watering
    private void NextDay()
    {
        if (_currentState == CropState.ReadyToHarvest) return;
        GrowPlants();

        _isWatered = false;
        _wateredSoilSR.sprite = null;
    }

    // Formats the object name for debugging
    private string FormatName() => _currentState == CropState.Planted
        ? $"{_tilemapName}-{_cropName} [{Location.x},{Location.y}]"
        : $"{_tilemapName} [{Location.x},{Location.y}]";

    // Checks if required components are missing
    private bool IsMissingRequiredComponents()
    {
        bool missing = false;
        if (_wateredSoilSR == null) { Debug.LogWarning("Missing watered soil SpriteRenderer"); missing = true; }
        if (_wateredSoilIcon == null) { Debug.LogWarning("Missing watered soil icon"); missing = true; }
        if (_plowedSoilSR == null) { Debug.LogWarning("Missing plowed soil SpriteRenderer"); missing = true; }
        if (_plowedSoilIcon == null) { Debug.LogWarning("Missing plowed soil icon"); missing = true; }

        return missing;
    }
}