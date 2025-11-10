using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedPacket", menuName = "Farming/Seed Packet")]
public class SeedPacket : ScriptableObject
{
    public string CropName;
    public Sprite[] GrowthSprites; // 0 = Seed, 1 = Sprout, 2 = Grown, 3 = Mature
    public GameObject HarvestPrefab;

    // Optional: adjust how fast crops grow
    public int DaysPerStage = 1;

    // Defines each growth stage
    public enum GrowthStage
    {
        Seed,
        Sprout,
        Grown,
        Mature
    }

    // Returns the appropriate sprite for the current stage
    public Sprite GetIconForStage(GrowthStage stage)
    {
        int index = (int)stage;
        if (GrowthSprites == null || GrowthSprites.Length == 0)
            return null;
        if (index >= GrowthSprites.Length)
            index = GrowthSprites.Length - 1;
        return GrowthSprites[index];
    }
}
