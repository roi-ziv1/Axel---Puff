using System;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    [SerializeField] private float shotSpeed = 13f; // Speed of the water shot
    // [SerializeField] private float shotDuration = 2f; // Duration for which the shot remains active
    // [SerializeField] private GameObject waterSplashPrefab; // Prefab for the water splash effect
    private Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * shotSpeed; // Set the velocity of the water shot
    }
    
    // Destroy the water shot when it collides with another object
    void OnCollisionEnter2D(Collision2D other)
    {
        // Instantiate the water splash effect at the point of collision
        // Instantiate(waterSplashPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the water shot after a certain duration
        Destroy(gameObject);
    }
}
