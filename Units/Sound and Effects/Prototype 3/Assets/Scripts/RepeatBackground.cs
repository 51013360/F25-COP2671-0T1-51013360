using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    // Initialize variables
    private Vector3 startPos;
    private float repeatWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get start position
        startPos = transform.position;

        // Get half the width of the backgound object as the background repeats twice
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        // If backgound moves halfway to the left
        if (transform.position.x < startPos.x - repeatWidth)
        {
            // Bring it back to the original position to create illusion of infinite movement
            transform.position = startPos;
        }
    }
}
