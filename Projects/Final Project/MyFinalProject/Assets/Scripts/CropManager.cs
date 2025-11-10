using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

// Ensures that a Grid component is attached to the GameObject
[RequireComponent(typeof(Grid))]
public class CropManager : MonoBehaviour
{
    // Prefab used to instantiate crop blocks on the grid
    [SerializeField] private CropBlock _cropBlockPrefab;

    // List of crops that have been planted by the player
    [SerializeField] private List<CropBlock> _plantedCrops = new();

    // Reference to the Grid component used for positioning
    private Grid _cropGrid;

    // Stores crop blocks for each tilemap as 2D arrays
    private List<CropBlock[,]> _cropGrids = new List<CropBlock[,]>();

    // Called when the script instance is being loaded
    void Awake()
    {
        // Get the Grid component attached to this GameObject
        _cropGrid = GetComponent<Grid>();
    }

    // Called before the first frame update
    private void Start()
    {
        // Get all child Tilemaps under the Grid
        var tilemaps = _cropGrid.GetComponentsInChildren<Tilemap>();
        if (tilemaps.Length == 0) return;

        foreach (Tilemap tilemap in tilemaps)
        {
            // Disable the visual rendering of the tilemap
            if (tilemap.TryGetComponent(out TilemapRenderer tilemapRenderer))
                tilemapRenderer.enabled = false;

            // Generate crop blocks based on the tilemap layout
            _cropGrids.Add(GenerateGridUsingTilemap(tilemap));
        }
    }

    // Called in the Unity Editor when values are changed in the Inspector
    private void OnValidate()
    {
        // Warn if the crop block prefab is missing
        if (_cropBlockPrefab == null)
        {
            Debug.LogWarning($"[{nameof(CropManager)}] CropBlock prefab is not assigned.", this);
        }

        // Ensure the Grid component is present
        if (GetComponent<Grid>() == null)
        {
            Debug.LogError($"[{nameof(CropManager)}] Missing required Grid component.", this);
        }
    }

    // Generates a 2D array of CropBlocks based on the tilemap's layout
    private CropBlock[,] GenerateGridUsingTilemap(Tilemap tilemap)
    {
        // Shrinks the tilemap bounds to fit only used tiles
        tilemap.CompressBounds();

        var bounds = tilemap.cellBounds;
        var width = bounds.size.x;
        var height = bounds.size.y;
        var offset = bounds.min;

        var gridBlocks = new CropBlock[width, height];

        // Iterate through each cell in the tilemap bounds
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Convert local array index to tilemap cell position
                var cellPosition = new Vector3Int(x + offset.x, y + offset.y, 0);

                // Only create a crop block if a tile exists at this position
                if (tilemap.HasTile(cellPosition))
                {
                    var localGridPosition = new Vector2Int(x, y);
                    gridBlocks[x, y] = CreateGridBlock(tilemap, localGridPosition, cellPosition);
                }
            }
        }

        return gridBlocks;
    }

    // Instantiates and initializes a CropBlock at the given position
    private CropBlock CreateGridBlock(Tilemap tilemap, Vector2Int location, Vector3Int position)
    {
        // Instantiate the crop block prefab at the specified position
        var newGridBlock = Instantiate(_cropBlockPrefab, position, Quaternion.identity, tilemap.transform);

        // Initialize the crop block with its tilemap name, location, and manager reference
        newGridBlock.Initialize(tilemap.name, location, this);

        // Disable interaction if the tilemap is not tagged as "PlayerCrop"
        if (tilemap.CompareTag("PlayerCrop") == false)
            newGridBlock.PreventUse();

        return newGridBlock;
    }

    // Adds a crop block to the list of planted crops if it's not already present
    public void AddToPlantedCrops(CropBlock cropBlock)
    {
        if (CheckForValidLocation(cropBlock.Location))
            _plantedCrops.Add(cropBlock);
    }

    // Removes a crop block from the list of planted crops based on its location
    public void RemoveFromPlantedCrops(Vector2Int location)
    {
        var cropBlock = _plantedCrops.SingleOrDefault(q => q.Location == location);
        if (cropBlock != null)
            _plantedCrops.Remove(cropBlock);
    }

    // Checks if a location is available for planting (i.e., not already occupied)
    public bool CheckForValidLocation(Vector2Int location)
    {
        return _plantedCrops.Any(q => q.Location == location) == false;
    }
}
