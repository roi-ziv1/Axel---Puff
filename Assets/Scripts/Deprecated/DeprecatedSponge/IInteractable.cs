using UnityEngine;

namespace SpongeScene
{
    public interface IInteractable
    {
        Transform objectTransform { get; }
        void OnInteract(); // Defines an interaction behavior
        void OnDrop();     // Defines behavior when the object is dropped
    }

    public interface IAbsorbable
    {
        void OnAbsorb();
        void Absorb();  // Behavior when absorbed by an object like Sponge

        void OnRelease();
        void Release();
    }
}