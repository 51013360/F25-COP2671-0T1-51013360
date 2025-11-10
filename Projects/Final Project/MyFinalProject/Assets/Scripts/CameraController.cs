using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // Player to follow
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector2 minBounds;     // Bottom-left map limit
    [SerializeField] private Vector2 maxBounds;     // Top-right map limit

    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        // Calculate camera bounds based on orthographic size and aspect ratio
        Camera cam = GetComponent<Camera>();
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Follow player
        Vector3 desiredPosition = target.position;

        // Clamp camera so it stays within map bounds
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

        Vector3 smoothedPosition = new Vector3(clampedX, clampedY, transform.position.z);
        transform.position = smoothedPosition;
    }
}
