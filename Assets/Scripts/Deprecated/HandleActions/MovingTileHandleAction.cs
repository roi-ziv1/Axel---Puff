// using DoubleTrouble.Interfaces;
// using DoubleTrouble.Managers;
// using UnityEngine;
//
// namespace Deprecated.HandleActions
// {
//     public class MovingTileHandleAction : MonoBehaviour, IHandleAction
//     {
//         [SerializeField] private LayerMask groundLayer; // Assign the "Ground" layer in the Inspector
//         private bool move;
//         private Vector2 currentDirection; // Stores the current direction of movement
//         private float speed;
//         private BoxCollider2D boxCollider;
//
//         private void Start()
//         {
//             speed = 2f;
//             boxCollider = GetComponent<BoxCollider2D>(); // Cache the BoxCollider2D
//         }
//
//         private void Update()
//         {
//             if (move)
//             {
//                 // Move the tile in the current direction
//                 transform.position += (Vector3)currentDirection * (speed * Time.deltaTime);
//
//                 // Check if the platform should stop moving
//                 CheckForGroundCollision();
//             }
//         }
//
//         public void Activate(Vector2 activationDirection)
//         {
//             // Set the direction of movement
//             if (activationDirection == Vector2.right || activationDirection == Vector2.left)
//             {
//                 currentDirection = activationDirection;
//                 move = true;
//             }
//         }
//
//         private void CheckForGroundCollision()
//         {
//             // Calculate ray origin based on the direction
//             Vector3 rayOrigin = transform.position +
//                                 (Vector3)currentDirection * (boxCollider.bounds.extents.x + 0.1f);
//
//             // Raycast to detect ground in the movement direction
//             float rayDistance = 0.2f; // Slightly beyond the edge of the platform
//             bool groundDetected = Physics2D.Raycast(rayOrigin, currentDirection, rayDistance, groundLayer);
//
//             // Debug visualization
//             Debug.DrawRay(rayOrigin, currentDirection * rayDistance, groundDetected ? Color.green : Color.red);
//
//             // Stop moving if ground is detected
//             if (groundDetected)
//             {
//                 move = false;
//             }
//         }
//
//         private void OnCollisionStay2D(Collision2D other)
//         {
//             // Check if the player is on the platform
//             if (other.gameObject.GetComponent<Player>())
//             {
//                 // Move the player along with the platform
//                 other.transform.position += (Vector3)currentDirection * (speed * Time.deltaTime);
//             }
//         }
//     }
// }
