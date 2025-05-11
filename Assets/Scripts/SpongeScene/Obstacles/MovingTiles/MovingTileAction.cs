using System;
using Character;
using SpongeScene.Managers;
using UnityEngine.EventSystems;

namespace SpongeScene.ButtonAction
{
    using DoubleTrouble.Interfaces;
using DoubleTrouble.Managers;
using UnityEngine;

namespace Deprecated.HandleActions
{
    public class MovingTileAction : MonoBehaviour, IHandleAction
    {
        [SerializeField] private LayerMask groundLayer; // Assign the "Ground" layer in the Inspector
        [SerializeField] private float speed;
        [SerializeField] private Vector2 currentDirection; // Stores the current direction of movement
        [SerializeField] private float maxDistance;
        [SerializeField] private bool canChangeDirByButton;
        private Vector2 startingDirection; // Stores the current direction of movement
        private bool move;
        private bool wasAlreadyActivated;
        private Collider2D _collider;
        private Vector3 startingPos;


        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, Reset);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, Reset);
        }

        private void Reset(object obj)
        {
            currentDirection = startingDirection;
            move = false;
            transform.position = startingPos;
            wasAlreadyActivated = false;
        }

        private void Start()
        {
            move = false;
            startingPos = transform.position;
            _collider = GetComponent<Collider2D>(); // Cache the BoxCollider2D
            startingDirection = currentDirection;
        }

        private void Update()
        {
            if (move)
            {
                // Move the tile in the current direction
                transform.position += (Vector3)currentDirection * (speed * Time.deltaTime);
                if (Vector3.Distance(startingPos, transform.position) > maxDistance)
                {
                    move = false;
                }
                // Check if the platform should stop moving
                // CheckForGroundCollision();
            }
        }

        public void Activate(Vector2 direction)
        {
            move = true;
            print("ACTIVATE CALLED!");
            if (canChangeDirByButton)
            {
                currentDirection = direction;
                return;
            }
            
            if (wasAlreadyActivated)  // make sure to switch direction when pressed again
            {
                currentDirection *= -1;
            }
            if (!wasAlreadyActivated)
            {
                wasAlreadyActivated = true;
            }

        }

        public void Deactivate(Vector2 direction)
        {
            return;
        }

        private void CheckForGroundCollision()
        {
            // Calculate ray origin based on the direction
            print("CHECK FOR GLUND COLISION");
            Vector3 rayOrigin = transform.position +
                                (Vector3)currentDirection * (_collider.bounds.extents.y + 0.1f);

            // Raycast to detect ground in the movement direction
            float rayDistance = 0.2f; // Slightly beyond the edge of the platform
            bool groundDetected = Physics2D.Raycast(rayOrigin, currentDirection, rayDistance, groundLayer);

            // Debug visualization
            Debug.DrawRay(rayOrigin, currentDirection * rayDistance, groundDetected ? Color.green : Color.red);

            // Stop moving if ground is detected
            if (groundDetected)
            {
                currentDirection *= -1;
                print("GROUND DETECTED!!!");
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            // Check if the player is on the platform
            if (currentDirection == Vector2.right)
            {
                var player = other.gameObject.GetComponent<PlayerManager>();
                if (player is not null && other.transform.position.y > transform.position.y && move)
                {
                    // Move the player along with the platform
                    player.transform.position += (Vector3)currentDirection * (speed * Time.deltaTime);
                }
            }
        }
    }
}

}