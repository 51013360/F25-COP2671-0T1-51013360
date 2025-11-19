using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public CropBlock cropBlock;

    //public GameObject cropBlockPrefab; 
    //public Tilemap cropGrid;       
    //public Transform playerTransform; 

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            //SpawnAtClosestTile();
            cropBlock.PlowCrop();
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            cropBlock.WaterCrop();
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            cropBlock.PlantCrop();
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            cropBlock.HarvestCrop();
    }

    //void SpawnAtClosestTile()
    //{
    //    Vector3 playerPosition = playerTransform.position;

    //    Vector3Int cellPosition = cropGrid.WorldToCell(playerPosition);

    //    Vector3 spawnPosition = cropGrid.CellToWorld(cellPosition);

    //    Instantiate(cropBlockPrefab, spawnPosition, Quaternion.identity);
    //}
}
