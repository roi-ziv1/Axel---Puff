using UnityEngine;

namespace SpongeScene.Utilities
{
    public class VisibilityHandler : MonoBehaviour
    {
        private Renderer renderer;

        void Start()
        {
            renderer = GetComponent<Renderer>();
        }

        void OnBecameInvisible()
        {
            // Disable rendering when the object is off-screen
            renderer.enabled = false;
        }

        void OnBecameVisible()
        {
            // Re-enable rendering when the object is on-screen
            renderer.enabled = true;
        }
    }
}
