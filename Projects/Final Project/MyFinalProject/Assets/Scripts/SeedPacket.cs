using UnityEngine;

[CreateAssetMenu(fileName = "New SeedPacket", menuName = "Farm/SeedPacket")]
public class SeedPacket : ScriptableObject
{
    public string cropName;
    [Tooltip("Sprites representing growth stages in order (0..3)")]
    public Sprite[] growthSprites; // expected length 4
    public Sprite coverImage;
    public GameObject harvestablePrefab; // prefab spawned when harvested (optional)
    [Tooltip("Seconds per growth stage when watered. Use larger for longer growth.")]
    public float secondsPerStage = 120f;
}