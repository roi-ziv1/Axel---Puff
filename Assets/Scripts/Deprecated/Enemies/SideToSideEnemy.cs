using DoubleTrouble.Interfaces;
using DoubleTrouble.Managers;
using UnityEngine;

namespace DoubleTrouble.Enemies
{
    public class SideToSideEnemy : Enemy
    {
        
        [Header("Movement Settings")]
        [SerializeField] private float rayDistance = 1f;

        [Header("Detection Layers")]
        [SerializeField] private LayerMask groundLayer;

        private Vector2 moveDirection = Vector2.right; // Initial movement direction

        
        private void FixedUpdate()
        {
            Move();
            CheckPlatformEdge();
        }

        protected override void Move()
        {
            // Apply movement
            rb.linearVelocity = new Vector2(moveDirection.x * speed, rb.linearVelocity.y);
        }

        protected override void Attack()
        {
            return;
        }

        private void CheckPlatformEdge()
        {
            // Cast a ray downwards at the edge of the enemy
            Vector2 rayOrigin = (Vector2)transform.position + moveDirection * 0.5f; // Offset for raycasting from the front
            RaycastHit2D groundHit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, groundLayer);

            // Debug raycast
            Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, groundHit ? Color.green : Color.red);

            // If no ground detected, reverse direction
            if (!groundHit.collider)
            {
                FlipDirection();
            }
        }

        private void FlipDirection()
        {
            // Reverse movement direction
            moveDirection *= -1;

            // Flip the enemy's sprite
            transform.localScale = new Vector3(
                transform.localScale.x * -1, 
                transform.localScale.y, 
                transform.localScale.z
            );
        }

        
        
    }
}