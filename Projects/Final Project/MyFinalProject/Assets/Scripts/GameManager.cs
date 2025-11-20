using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Tilemap cropGrid;
    public Transform player;

    public GameObject cropBlockPrefab;
    public GameObject seedlingPrefab;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CropBlock target = GetClosestCropBlock();
            if (target == null)
                target = SpawnCropBlock();

            target.PlowCrop();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            CropBlock target = GetClosestCropBlock();
            if (target != null)
                target.WaterCrop();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            CropBlock target = GetClosestCropBlock();
            if (target != null)
                target.PlantCrop();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            CropBlock target = GetClosestCropBlock();
            if (target != null)
                target.HarvestCrop();
        }
    }

    private CropBlock SpawnCropBlock()
    {
        Vector3Int cellPos = cropGrid.WorldToCell(player.position);
        Vector3 spawnPos = cropGrid.GetCellCenterWorld(cellPos);
        spawnPos.z = 0;

        GameObject obj = Instantiate(cropBlockPrefab, spawnPos, Quaternion.identity);
        return obj.GetComponent<CropBlock>();
    }

    private CropBlock GetClosestCropBlock()
    {
        CropBlock[] allBlocks = FindObjectsByType<CropBlock>(FindObjectsSortMode.None);

        if (allBlocks.Length == 0)
            return null;

        CropBlock closest = null;
        float closestDist = float.MaxValue;

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
