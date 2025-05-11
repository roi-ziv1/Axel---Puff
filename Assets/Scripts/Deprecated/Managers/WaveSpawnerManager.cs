// namespace DoubleTrouble.Managers
// {
//     using UnityEngine;
//
// namespace DoubleTrouble.Managers
// {
//     public class WaveSpawnerManager : MonoBehaviour
//     {
//         // Call this method to spawn enemies for the current wave
//         public void SpawnEnemy(SpawnType spawnType, int enemyCount)
//         {
//             for (int i = 0; i < enemyCount; i++)
//             {
//                 if (spawnTimer <= 0)
//                 {
//                     // Instantiate an enemy based on the SpawnType
//                     Vector3 spawnPosition = GetSpawnPosition(spawnType);
//
//                     Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
//                     spawnTimer = spawnDelay;
//                 }
//             }
//         }
//
//         // Get spawn position based on SpawnType
//         private Vector3 GetSpawnPosition(SpawnType spawnType)
//         {
//             switch (spawnType)
//             {
//                 case SpawnType.Left:
//                     return new Vector3(-10f, Random.Range(-5f, 5f), 0f); // Left side of the screen
//                 case SpawnType.Right:
//                     return new Vector3(10f, Random.Range(-5f, 5f), 0f); // Right side of the screen
//                 case SpawnType.Air:
//                     return new Vector3(Random.Range(-5f, 5f), 10f, 0f); // Air (above screen)
//                 case SpawnType.Ceiling:
//                     return new Vector3(Random.Range(-5f, 5f), 5f, 0f); // Ceiling
//                 case SpawnType.Ground:
//                     return new Vector3(Random.Range(-5f, 5f), -5f, 0f); // Ground level
//                 default:
//                     return Vector3.zero;
//             }
//         }
//
//         // Get the appropriate prefab based on the SpawnType
//         private GameObject GetEnemyPrefab(SpawnType spawnType)
//         {
//             // Choose enemy prefab based on the type
//             switch (spawnType)
//             {
//                 case SpawnType.Left:
//                 case SpawnType.Right:
//                     return enemyPrefabs[0]; // Ground enemy
//                 case SpawnType.Air:
//                     return enemyPrefabs[1]; // Flying enemy
//                 case SpawnType.Ceiling:
//                     return enemyPrefabs[2]; // Ceiling enemy
//                 case SpawnType.Ground:
//                     return enemyPrefabs[3]; // Ground-based enemy
//                 default:
//                     return enemyPrefabs[0];
//             }
//         }
//     }
// }
//
// }