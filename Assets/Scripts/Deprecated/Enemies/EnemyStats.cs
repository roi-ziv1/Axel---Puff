using UnityEngine;

namespace DoubleTrouble.Managers
{
    [CreateAssetMenu(fileName = "NewEnemyStats", menuName = "DoubleTrouble/Enemy Stats", order = 1)]
    public class EnemyStats : ScriptableObject
    {
        public int health;
        public int damage;
        public int moveSpeed;
    }
}