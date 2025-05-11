using System;
using UnityEngine;

namespace SpongeScene.WaterCart
{
    public class Cart : MonoBehaviour
    {
        public Transform[] wheels; // Array of wheel transforms
        public float wheelRotationSpeed = 30f; // Speed of wheel rotation based on applied force

        [SerializeField] private float forcePerParticle;
        private Vector3 velocity; // Manually track the cart's velocity
        private float friction = 0.95f; // Simulate friction for gradual slowing

        private void Update()
        {
            // Apply friction to velocity for gradual slowing
            velocity *= friction;
            transform.position += velocity * Time.deltaTime;

            // Update wheel rotation based on velocity magnitude
            RotateWheels(velocity.magnitude);
        }

        private void OnParticleCollision(GameObject other)
        {
            Vector3 hitDir = (transform.position - other.transform.position).normalized;
            Vector3 dirWithoutY = new Vector3(hitDir.x, 0, 0);
            velocity += dirWithoutY * forcePerParticle;

            // Rotate wheels based on the magnitude of the applied force
            RotateWheels(forcePerParticle);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.CompareTag("Tracks"))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        private void RotateWheels(float forceMagnitude)
        {
            // Rotate each wheel based on the applied force magnitude
            float rotationAmount = forceMagnitude * wheelRotationSpeed * Time.deltaTime;
            foreach (var wheel in wheels)
            {
                if (wheel != null)
                {
                    wheel.Rotate(Vector3.forward, rotationAmount);
                }
            }
        }
    }
}