using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Initialize variables
    public float speed = 3.0f;
    private Rigidbody enemyRb;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get enemy and player objectss
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Make enemy follow player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        // If enemy falls off scene, destroy object
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
