using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Initialize variables
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerUpIndicator;
    public float speed = 5.0f;
    public bool hasPowerUp = false;
    private float powerUpStrength = 15.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get enemy and player objects
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        // Make player move from input
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        // Make power-up indicator follow player position lower on the ground
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If power-up object collides with player
        if (other.CompareTag("PowerUp"))
        {
            // Indicate player got power-up
            hasPowerUp = true;
            
            // Make power-up indicator visible
            powerUpIndicator.gameObject.SetActive(true);

            // Destroy power-up item
            Destroy(other.gameObject);

            // Start power-up countdown for how long it will last
            StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        // Wait for 7 seconds before deactivating power-up
        yield return new WaitForSeconds(7);

        // Set player power-up indication to false
        hasPowerUp = false;
        powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            // Initialize variables
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            // Let user know player got the power-up
            Debug.Log("Collided with " + collision.gameObject.name + " with power-up set to " + hasPowerUp);
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
