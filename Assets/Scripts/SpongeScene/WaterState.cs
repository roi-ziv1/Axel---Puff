using UnityEngine;

public class WaterState : MonoBehaviour
{
    public bool IsFired { get; private set; } = false; // Indicates whether the water was fired
    [SerializeField] private float activeDuration = 5f; // Default duration for which fired water remains active

    /// <summary>
    /// Sets the water state to "fired". Fired water remains active for a set duration
    /// and can trigger special interactions, like growing objects.
    /// </summary>
    public void SetFired()
    {
        IsFired = true;
        // Automatically disable the effect after the specified duration
        Invoke(nameof(DisableEffect), activeDuration);
    }

    /// <summary>
    /// Disables the fired effect, making the water inactive.
    /// This prevents further interactions from this water droplet.
    /// </summary>
    public void DisableEffect()
    {
        IsFired = false; // Mark the water as inactive
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If water hits an object with the "Ground" tag, schedule evaporation
        if (collision.gameObject.CompareTag("Ground"))
        {
            float evaporationTime = Random.Range(2f, 10f); // Random time between 2 and 10 seconds
            Invoke(nameof(Evaporate), evaporationTime);
        }
        // If water hits an object with the "Pool" tag, do nothing (water remains)
        else if (collision.gameObject.CompareTag("Pool"))
        {
            CancelInvoke(nameof(Evaporate)); // Ensure the water doesn't evaporate
        }
    }

    /// <summary>
    /// Handles the evaporation of the water droplet.
    /// This method is invoked after a random delay when water hits the ground.
    /// </summary>
    private void Evaporate()
    {
        // Optionally, play evaporation animation or sound effect here
        Destroy(gameObject); // Remove the water droplet from the scene
    }
}