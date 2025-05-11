using System;
using System.Collections;
using Character;
using SpongeScene.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class RandomShooterObstacle : ShooterObstacle
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