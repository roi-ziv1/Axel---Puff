using System.Collections;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class ShurikenObstacle : ShooterObstacle
    {

        [SerializeField] private float cd;
        

        public override IEnumerator Shoot()
        {
            while (true)
            {
                foreach (var shooter in shooters)
                {
                    shooter.Shoot();
                }
                yield return new WaitForSeconds(cd);

            }
        }
    }
}