using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Initialize variables
    private Rigidbody playerRb; 
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // If space bar is pressed, player is on ground, and game is not over
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            // Make player go up in y position, set animation to jump, stop dirt particle, make jump noise
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 0.7f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If player is on ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Display dirst particles
            isOnGround = true;
            dirtParticle.Play();
        }
        // If player collides with the obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Display "Game Over!" in debug log, play death animation, play explosion animation, play crash sound, stop dirtparticle
            gameOver = true;
            Debug.Log("Game Over!");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 0.7f);
        }
    }
}
