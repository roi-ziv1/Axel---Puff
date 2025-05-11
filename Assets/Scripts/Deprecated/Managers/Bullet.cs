using System;
using DoubleTrouble.Interfaces;
using UnityEngine;

namespace DoubleTrouble.Managers
{
    public class Bullet : MonoBehaviour, IDealDamage
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        [SerializeField] private Rigidbody2D rb;
        private void OnEnable()
        {
            rb = GetComponent<Rigidbody2D>();
            // move the kinematic rigidbody
            rb.linearVelocity = transform.right * speed;
            Destroy(gameObject, lifeTime);
        }

        public void DealDamage(ITakeDamage damaged)
        {
            damaged.TakeDamage(this);
        }

        public int GetDamage()
        {
            return 4;
        }

        public GameObject GetDamageSource()
        {
            return gameObject;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
                DealDamage(enemy);
            Destroy(gameObject);
        }
    }
}
