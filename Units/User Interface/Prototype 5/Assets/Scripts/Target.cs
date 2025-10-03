using UnityEngine;

public class Target : MonoBehaviour
{
    // Initialize variables
    private Rigidbody targetRb;
    private GameManager gameManager;
    public ParticleSystem explosionParticle;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 1;
    private float xRange = 4;
    private float ySpawnPos = -2;

    public int pointValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get target object components
        targetRb = GetComponent<Rigidbody>();

        // Spawn targets to pop up and spin at random speeds
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();

        // Get Game Manager object component
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        // If isGameActive is true
        if (gameManager.isGameActive)
        {
            // Destroy object when clicked
            Destroy(gameObject);
            
            // Display particle explosion
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

            // Update score
            gameManager.UpdateScore(pointValue);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If target falls below certain Y value, destroy object
        Destroy(gameObject);

        // If target is not a "bad" one
        if (!gameObject.CompareTag("Bad"))
        {
            // Set game as over
            gameManager.GameOver();
        }
    }

    // Methods to return random speeds for force, torque and spawn positions
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
