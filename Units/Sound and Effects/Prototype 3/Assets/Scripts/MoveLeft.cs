using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Initialize variables
    private float speed = 30;
    private PlayerController playerControllerScript;
    private float leftBound = -15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Bring over elements from PlayerController script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // If game is not over
        if (playerControllerScript.gameOver == false)
        {
            // Continue moving everything this script is attached to, to the left
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        // If obstacle moves off screen to the left a certain distance
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            // Delete the obstacle
            Destroy(gameObject);
        }
    }
}
