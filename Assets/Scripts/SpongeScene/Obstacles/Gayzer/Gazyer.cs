using UnityEngine;

namespace SpongeScene.WaterTriggers
{
    public class Gazyer : MonoBehaviour
    {
        public void ShootGayzerProxy()
        {
            transform.parent?.GetComponent<GayzerMekanism>()?.ShootGayzer();
        }
    }
}