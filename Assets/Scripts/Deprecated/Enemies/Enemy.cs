using System;
using DoubleTrouble.Interfaces;
using UnityEngine;

namespace DoubleTrouble.Managers
{
    public abstract class Enemy : MonoBehaviour, IAttackable
    {
        public EnemyStats stats; // The reference to the ScriptableObject containing enemy data

        public bool IsAlive { get; set; }
        public EnemyType Type => type;

        protected Rigidbody2D rb;
        protected float currentHealth;
        protected float speed;
        protected int damage;
        protected float attackRange;
        protected float attackCooldown;
        protected EnemyType type;
        [SerializeField] private bool isAlive;

        protected virtual void Awake()
        {
            // Initialize values from the EnemyStats ScriptableObject
            if (stats != null)
            {
                currentHealth = stats.health;
                speed = stats.moveSpeed;
                damage = stats.damage;
                // attackRange = stats.attackRange; // Assuming this is part of the stats
                // attackCooldown = stats.attackCooldown; // Assuming this is part of the stats
            }
            else
            {
                Debug.LogWarning("EnemyStats ScriptableObject is not assigned to this enemy.");
            }
            isAlive = true;
            rb = GetComponent<Rigidbody2D>();
        }

        // Abstract method to handle movement logic, must be implemented in subclasses
        protected abstract void Move();

        // Abstract method to handle attack logic, must be implemented in subclasses
        protected abstract void Attack();

        // Abstract method for taking damage logic, subclasses can implement different death behavior
        public virtual void TakeDamage(IDealDamage damager)
        {
            if (damager == null)
                return;
            currentHealth -= damager.GetDamage();
            Debug.Log($"{gameObject.name} took damage");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void DealDamage(ITakeDamage damaged)
        {
            if (damaged == null)
                return;
            
            
            damaged.TakeDamage(this);
            Debug.Log($"{gameObject.name} dealt {damage} damage to {damaged}");
            isAlive = false;
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
        
        public int GetDamage()
        {
            return damage;
        }

        public GameObject GetDamageSource()
        {
            return this.gameObject;
        }

        // Abstract method for enemy death logic, subclasses can provide custom death behaviors
        public void Die()
        {
            print("DIE ENEMY DIE");
            isAlive = false;
            gameObject.SetActive(false);
        }

        // You can also choose to provide default implementations of non-abstract methods
        // that might be shared across all enemies, like some common attack behaviors.

        private void Update()
        {
            Move();
        }




    }
}