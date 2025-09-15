using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Variables
    private float topBound = 30;
    private float lowerBound = -10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If an object goes past the player's view in the game, remove that object
        if (transform.position.z > topBound)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < lowerBound)
        {
            // Notify player that the game ended
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }
}
