using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Initialize variables
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    private float spawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Call spawn enemy wave function
        SpawnEnemyWave(waveNumber);

        // Spawn power-up on random coordinate
        Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for enemy count on scene
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

        // If enemy on scene is 0
        if (enemyCount == 0)
        {
            // Increase enemy spawn number
            waveNumber++;
            
            // Spawn more enemies
            SpawnEnemyWave(waveNumber);

            // Spawn more power-up
            Instantiate(powerUpPrefab, GenerateSpawnPosition(), powerUpPrefab.transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        // For-loop for enemy waves
        for (int i = 0; i < enemiesToSpawn;  i++)
        {
            // Spawn enemies on random coordinates
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        // Random enemy spawn positions
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
}
