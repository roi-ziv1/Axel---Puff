using UnityEngine;

namespace SpongeScene.Triggers
{
    public abstract class Trigger : MonoBehaviour
    {
        // Indicates whether the trigger condition is currently met
        public abstract bool IsActivated();

        // Optional: Event for when the trigger condition becomes true

        // This method is called to manually check and invoke the trigger
        public bool CheckAndTrigger()
        {
            if (IsActivated())
            {
                return true;
            }

            return false;
        }
    }
}