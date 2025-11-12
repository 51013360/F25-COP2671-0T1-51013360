using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    public Tilemap tilemap; // assign SoilTilemap
    public GameObject cropBlockPrefab; // assign CropBlockPrefab

    // grid bounds from tilemap
    private BoundsInt mapBounds;
    private CropBlock[,] gridArray; // 2D array storage
    private Vector2Int offset; // to transform cell coords -> array indices
    private List<CropBlock> plantedCrops = new List<CropBlock>();

    void Awake()
    {
        if (tilemap == null) Debug.LogError("Tilemap not assigned to CropManager.");
        CreateGridUsingTilemap(tilemap);
    }

    public void CreateGridUsingTilemap(Tilemap tmap)
    {
        mapBounds = tmap.cellBounds;
        int width = mapBounds.size.x;
        int height = mapBounds.size.y;
        gridArray = new CropBlock[width, height];
        offset = new Vector2Int(mapBounds.xMin, mapBounds.yMin);

        // Optionally, create GridBlocks for existing soil tiles (but we will create on demand)
        // For now we leave entries null and create when planting
    }

    // create and return a CropBlock at tile-location (Vector2Int cell)
    public CropBlock CreateGridBlock(Tilemap tmap, Vector2Int cellLocation, Vector3 worldPosition)
    {
        Vector2Int idx = CellToIndex(cellLocation);
        if (!IndexInRange(idx)) return null;

        if (gridArray[idx.x, idx.y] != null) return gridArray[idx.x, idx.y];

        GameObject go = Instantiate(cropBlockPrefab, worldPosition, Quaternion.identity, transform);
        CropBlock cb = go.GetComponent<CropBlock>();
        cb.GridLocation = cellLocation;
        gridArray[idx.x, idx.y] = cb;
        return cb;
    }

    public void AddToPlantedCrops(CropBlock cropBlock)
    {
        if (!plantedCrops.Contains(cropBlock)) plantedCrops.Add(cropBlock);
    }

    public void RemoveFromPlantedCrops(CropBlock cropBlock)
    {
        if (plantedCrops.Contains(cropBlock)) plantedCrops.Remove(cropBlock);
    }

    // helper: convert cell coords to array index
    private Vector2Int CellToIndex(Vector2Int cell)
    {
        return new Vector2Int(cell.x - offset.x, cell.y - offset.y);
    }

    private bool IndexInRange(Vector2Int idx)
    {
        return idx.x >= 0 && idx.y >= 0 && idx.x < gridArray.GetLength(0) && idx.y < gridArray.GetLength(1);
    }

    // get world position for center of a tile cell
    public Vector3 CellToWorldCenter(Vector2Int cell)
    {
        Vector3Int cell3 = new Vector3Int(cell.x, cell.y, 0);
        return tilemap.CellToWorld(cell3) + tilemap.cellSize * 0.5f;
    }

    // helper to get a CropBlock at a tile or null
    public CropBlock GetCropBlockAtCell(Vector2Int cell)
    {
        Vector2Int idx = CellToIndex(cell);
        if (!IndexInRange(idx)) return null;
        return gridArray[idx.x, idx.y];
    }

    // Convenience: get cell from world position
    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
    }
}
