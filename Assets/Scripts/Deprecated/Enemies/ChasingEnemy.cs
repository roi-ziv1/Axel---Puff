// using System;
// using DoubleTrouble.Interfaces;
// using DoubleTrouble.Managers;
// using UnityEngine;
// using CoreManager = SpongeScene.Managers.CoreManager;
//
// namespace DoubleTrouble.Enemies
// {
//     public class ChasingEnemy : Enemy
//     {
//         private Transform targetPlayer; // Reference to the nearest player
//
//         private void Start()
//         {
//             FindNearestPlayer();
//         }
//
//         private void Update()
//         {
//             if (IsAlive && targetPlayer != null)
//             {
//                 Move();
//             }
//         }
//
//         // Find the nearest player based on the X value
//         private void FindNearestPlayer()
//         {
//             print($"core is null { CoreManager.Instance == null}");
//             if (CoreManager.Instance.players == null || CoreManager.Instance.players.Count == 0)
//             {
//                 Debug.Log("No players found in CoreManager.");
//                 return;
//             }
//
//             float closestDistance = Mathf.Infinity;
//             Transform closestPlayer = null;
//
//             foreach (var player in CoreManager.Instance.players)
//             {
//                 float distance = Mathf.Abs(transform.position.x - player.transform.position.x);
//
//                 if (distance < closestDistance)
//                 {
//                     closestDistance = distance;
//                     closestPlayer = player.transform;
//                 }
//             }
//
//             targetPlayer = closestPlayer;
//         }
//
//         // Move towards the nearest player
//         protected override void Move()
//         {
//             if (targetPlayer == null)
//                 return;
//
//             // Calculate direction and move towards the player
//             Vector3 direction = (targetPlayer.position - transform.position).normalized;
//             transform.position += direction * (speed * Time.deltaTime);
//         }
//
//         // Attack logic when colliding with a player
//         protected override void Attack()
//         {
//             if (targetPlayer == null)
//                 return;
//
//             if (targetPlayer.TryGetComponent<ITakeDamage>(out var damageable))
//             {
//                 DealDamage(damageable);
//             }
//         }
//
//         public override void TakeDamage(IDealDamage damager)
//         {
//             base.TakeDamage(damager);
//         }
//
//         public override void DealDamage(ITakeDamage damaged)
//         {
//             base.DealDamage(damaged);
//         }
//
//         // Handle collision detection
//         private void OnCollisionEnter2D(Collision2D collision)
//         {
//             // Handle collision with a player to deal damage
//             if (collision.gameObject.TryGetComponent<ITakeDamage>(out var playerDamageable))
//             {
//                 DealDamage(playerDamageable);
//             }
//             
//         }
//     }
// }
