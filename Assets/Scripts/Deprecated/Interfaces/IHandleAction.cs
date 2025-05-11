using UnityEngine;

namespace DoubleTrouble.Interfaces
{
    public interface IHandleAction
    {
        void Activate(Vector2 direction);
        void Deactivate(Vector2 direction);
    }
}