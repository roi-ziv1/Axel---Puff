using System;
using UnityEngine;

namespace DoubleTrouble.Health
{
     public class HealthBar : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool destroyOnDeath = true;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => maxHealth;

        public bool IsAlive => CurrentHealth > 0;

        // Events to notify external systems of health changes or death
        public event Action<int, int> OnHealthChanged; // Current health, Max health
        public event Action OnDeath;

        private void Awake()
        {
            CurrentHealth = maxHealth; // Initialize health
        }

        /// <summary>
        /// Applies damage to the object. If health reaches 0, triggers death.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        /// <param name="source">The GameObject that dealt the damage (optional).</param>
        public void TakeDamage(int amount, GameObject source = null)
        {
            if (!IsAlive) return;

            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

            if (CurrentHealth <= 0)
            {
                Die(source);
            }
        }

        /// <summary>
        /// Heals the object by a specified amount, up to its maximum health.
        /// </summary>
        /// <param name="amount">The amount of healing to apply.</param>
        public void Heal(int amount)
        {
            if (!IsAlive) return;

            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        /// <summary>
        /// Handles death logic.
        /// </summary>
        /// <param name="source">The GameObject that caused the death (optional).</param>
        private void Die(GameObject source)
        {
            OnDeath?.Invoke();

            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Resets the health to its maximum value.
        /// </summary>
        public void ResetHealth()
        {
            CurrentHealth = maxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        /// <summary>
        /// Sets the maximum health and adjusts current health proportionally.
        /// </summary>
        /// <param name="newMaxHealth">The new maximum health value.</param>
        public void SetMaxHealth(int newMaxHealth)
        {
            if (newMaxHealth <= 0) return;

            float healthPercentage = (float)CurrentHealth / maxHealth;
            maxHealth = newMaxHealth;
            CurrentHealth = Mathf.Clamp(Mathf.RoundToInt(maxHealth * healthPercentage), 0, maxHealth);

            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }
    }
}