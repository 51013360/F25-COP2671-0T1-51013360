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
        if (!TimeManager.isDay)
        {
            Debug.Log("Can only farm during the day");
            return;
        }

        CropBlock newBlock = SpawnCropBlock();
        if (newBlock != null) newBlock.PlowCrop();
    }

    public void WaterAction()
    {
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.WaterCrop();
    }

    public void PlantAction()
    {
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.PlantCrop();
    }

    public void HarvestAction()
    {
        CropBlock target = GetClosestCropBlock();
        if (target != null) target.HarvestCrop();
    }

    public CropBlock SpawnCropBlock()
    {
        Vector3Int cellPos = cropGrid.WorldToCell(player.position);
        Vector3 spawnPos = cropGrid.GetCellCenterWorld(cellPos);

        if (cropGrid.GetTile(cellPos) == null)
        {
            Debug.Log("Cannot plant here: no farmable tile.");
            return null;
        }

        spawnPos.z = 0;

        GameObject obj = Instantiate(cropBlockPrefab, spawnPos, Quaternion.identity);
        return obj.GetComponent<CropBlock>();
    }

    public CropBlock GetClosestCropBlock()
    {
        CropBlock[] allBlocks = FindObjectsByType<CropBlock>(FindObjectsSortMode.None);
        if (allBlocks.Length == 0) return null;

        CropBlock closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var block in allBlocks)
        {
            float dist = Vector3.Distance(player.position, block.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = block;
            }
        }

        return closest;
    }
}