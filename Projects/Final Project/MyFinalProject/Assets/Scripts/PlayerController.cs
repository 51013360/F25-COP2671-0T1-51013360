using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private float _playerSpeed = 5f;
    public Vector2 PlayerInput => _inputActionReference.action.ReadValue<Vector2>();

    private Vector2 _lastPlayerInput;
    private Rigidbody2D _rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _lastPlayerInput = PlayerInput.normalized;
    }

    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _lastPlayerInput * _playerSpeed;
    }
}
