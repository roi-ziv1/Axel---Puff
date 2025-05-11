using System.Collections;
using SpongeScene.Obstacles.ShootingObstacles;
using UnityEngine;

namespace Character
{
    public class ShardObstacleSingular : RandomShooterObstacle
    {
        
        public override IEnumerator Shoot()
        {
            foreach (var shooter in shooters)
            {
              shooter.Shoot();   
            }

            yield return null;
        }


        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
        }
    }
}