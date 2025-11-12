using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class FarmingController : MonoBehaviour
{
    public CropManager cropManager;
    public Tilemap soilTilemap;

    [HideInInspector] public CropBlock selectedBlock;
    [HideInInspector] public Vector3Int selectedCell;

    // Events (optional) you can hook into from UI or other systems
    public UnityEvent OnHoeEvent;
    public UnityEvent OnWaterEvent;
    public UnityEvent OnPlantEvent;
    public UnityEvent OnHarvestEvent;

    void Update()
    {
        // Select tile under mouse on left click (Editor / standalone)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = soilTilemap.WorldToCell(world);
            selectedCell = cell;

            Vector2Int cell2 = new Vector2Int(cell.x, cell.y);
            // Try to get CropBlock
            selectedBlock = cropManager.GetCropBlockAtCell(cell2);
            // If none, optionally create a placeholder when hoeing or planting will create later
        }
    }

    // Called by UI / toolbar when Hoe pressed
    public void OnHoe()
    {
        Vector2Int cell2 = new Vector2Int(selectedCell.x, selectedCell.y);
        // ensure cell contains soil tile
        if (soilTilemap.HasTile(selectedCell))
        {
            // create block if doesn't exist
            CropBlock cb = cropManager.GetCropBlockAtCell(cell2);
            if (cb == null)
            {
                Vector3 worldCenter = cropManager.CellToWorldCenter(cell2);
                cb = cropManager.CreateGridBlock(soilTilemap, cell2, worldCenter);
            }
            cb.TillSoil();
            selectedBlock = cb;
            OnHoeEvent?.Invoke();
        }
    }

    public void OnWater()
    {
        if (selectedBlock != null)
        {
            selectedBlock.WaterSoil();
            OnWaterEvent?.Invoke();
        }
    }

    // plant a specific seed packet
    public void OnPlant(SeedPacket seed)
    {
        if (seed == null) return;
        if (selectedBlock == null)
        {
            Vector2Int cell2 = new Vector2Int(selectedCell.x, selectedCell.y);
            if (soilTilemap.HasTile(selectedCell))
            {
                Vector3 worldCenter = cropManager.CellToWorldCenter(cell2);
                selectedBlock = cropManager.CreateGridBlock(soilTilemap, cell2, worldCenter);
            }
            else return;
        }
        selectedBlock.PlantSeed(seed);
        cropManager.AddToPlantedCrops(selectedBlock);
        OnPlantEvent?.Invoke();
    }

    public void OnHarvest()
    {
        if (selectedBlock != null)
        {
            selectedBlock.Harvest();
            cropManager.RemoveFromPlantedCrops(selectedBlock);
            OnHarvestEvent?.Invoke();
        }
    }
}

