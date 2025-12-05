using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private float _playerSpeed = 5f;
    public Vector2 PlayerInput => _inputActionReference.action.ReadValue<Vector2>();

    private Vector2 _lastPlayerInput;
    private Rigidbody2D _rigidBody;

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read and normalize player input
        _lastPlayerInput = PlayerInput.normalized;

        // Determine if the player is moving
        bool isMoving = _lastPlayerInput.sqrMagnitude > 0.01f;

        // Handle flipping for left movement
        if (_lastPlayerInput.x < 0)
            _spriteRenderer.flipX = true; // moving left
        else if (_lastPlayerInput.x > 0)
            _spriteRenderer.flipX = false; // moving right
        
        // Set animator parameters
        _animator.SetFloat("MoveX", _lastPlayerInput.x);
        _animator.SetFloat("MoveY", _lastPlayerInput.y);
        _animator.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        // Apply movement to the rigidbody
        _rigidBody.linearVelocity = _lastPlayerInput * _playerSpeed;
    }
}
