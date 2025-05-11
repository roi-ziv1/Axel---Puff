using UnityEngine;

namespace DoubleTrouble.Interfaces
{
    public interface IDealDamage
    {
        void DealDamage(ITakeDamage damaged);
        int GetDamage();
        GameObject GetDamageSource();
    }
}