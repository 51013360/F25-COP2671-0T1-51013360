using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public Tilemap cropGrid;
    public Transform player;
    public GameObject cropBlockPrefab;
    public CropBlock currentCropPrefab;

    private void Update()
    {
        // Input handling for farming actions
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            PlowAction();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            WaterAction();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            PlantAction();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            HarvestAction();
        }
    }

    public void PlowAction()
    {
        // Can only plow during the day
        if (!TimeManager.isDay)
        {
            Debug.Log("Can only farm during the day");
            return;
        }

        // Spawn a new crop block at the player's position and plow it
        CropBlock newBlock = SpawnCropBlock();
        if (newBlock != null) newBlock.PlowCrop();
    }

    public void WaterAction()
    {
        // Find the closest crop block to the player and water it
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.WaterCrop();
    }

    public void PlantAction()
    {
        //  Find the closest crop block to the player and plant a crop
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.PlantCrop();
    }

    public void HarvestAction()
    {
        // Find the closest crop block to the player and harvest it
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.HarvestCrop();
    }

    public CropBlock SpawnCropBlock()
    {
        // Determine the cell position based on the player's position
        Vector3Int cellPos = cropGrid.WorldToCell(player.position);
        Vector3 spawnPos = cropGrid.GetCellCenterWorld(cellPos);

        // Check if the tile at the cell position is farmable
        if (cropGrid.GetTile(cellPos) == null)
        {
            Debug.Log("Cannot plant here: no farmable tile.");
            return null;
        }

        spawnPos.z = 0;

        // Instantiate a new crop block at the calculated position
        GameObject obj = Instantiate(cropBlockPrefab, spawnPos, Quaternion.identity);
        return obj.GetComponent<CropBlock>();
    }

    public CropBlock GetClosestCropBlock()
    {
        // Find all crop blocks in the scene
        CropBlock[] allBlocks = FindObjectsByType<CropBlock>(FindObjectsSortMode.None);
        if (allBlocks.Length == 0) return null;

        // Determine the closest crop block to the player
        CropBlock closest = null;
        float closestDist = Mathf.Infinity;

        // Loop through all crop blocks to find which is nearest to the player
        foreach (var block in allBlocks)
        {
            // Calculate distance from the player to this crop block
            float dist = Vector3.Distance(player.position, block.transform.position);

            // If this distance is smaller than any previously found, update closest
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = block;
            }
        }

        return closest;
    }
}