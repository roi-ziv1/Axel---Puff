using UnityEngine;

public class SpringBall : MonoBehaviour
{
    // Store the initial local position of the ball
    private Vector3 initialLocalPosition;

    // Reference to the ball's Rigidbody2D
    private Rigidbody2D rb;

    // Adjustable parameters for the speed of return
    [SerializeField] private float returnSpeed = 0.5f; // Controls how fast the ball returns

    void Start()
    {
        // Save the ball's initial local position at the start
        initialLocalPosition = transform.localPosition;

        // Get the Rigidbody2D component attached to the ball
        rb = GetComponent<Rigidbody2D>();
    }

    // void FixedUpdate()
    // {
    //     // Convert the initial local position to world position
    //     Vector3 initialWorldPosition = transform.parent.TransformPoint(initialLocalPosition);
    //
    //     // Calculate the direction to the initial world position
    //     Vector3 directionToInitialPosition = initialWorldPosition - transform.position;
    //
    //     // Calculate the distance between the current position and the initial position
    //     float distance = directionToInitialPosition.magnitude;
    //
    //     // If the ball is far away, move it towards the initial position gradually
    //     if (distance > 0.1f)  // A small threshold to stop movement when close enough
    //     {
    //         // Move the Rigidbody2D using MovePosition, applying smooth movement
    //         Vector3 targetPosition = transform.position + directionToInitialPosition.normalized * (returnSpeed * Time.fixedDeltaTime);
    //         rb.MovePosition(targetPosition);
    //     }
    // }
}