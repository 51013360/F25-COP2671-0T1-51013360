using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Initialize variables
    public GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Continuously spawn obstacles at a set interval after a delay
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);

        // Bring over elements from PlayerController script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method that spawns obstacle prefab, which is invoked repeatedly in the Start() method
    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
    }
}
