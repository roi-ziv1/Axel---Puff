using System.Collections;
using Character;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class ShardObstacleMultiple : ShardObstacleSingular
    {
        public float shootInterval = 2f; // Configurable interval
        public int numberOfShots = 3;   // Number of shots for each burst

        public override IEnumerator Shoot()
        {
            for (int i = 0; i < numberOfShots; i++)
            {
                yield return StartCoroutine(base.Shoot());
                yield return new WaitForSeconds(shootInterval);
            }
        }
    }

}