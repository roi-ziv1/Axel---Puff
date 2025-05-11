// using System;
// using DoubleTrouble.Managers;
// using UnityEngine;
// using CoreManager = SpongeScene.Managers.CoreManager;
//
// namespace DoubleTrouble.Camera
// {
//     public class CameraFollower : MonoBehaviour
//     {
//         private UnityEngine.Camera mainCamera;
//         private bool move = true;
//
//         private void Awake()
//         {
//             print("CAM FOLLOWER INIT!");
//
//             mainCamera = CameraManager.Instance.MainCamera;
//         }
//
//         void LateUpdate()
//         {
//             if (!move) return;
//             if ( CoreManager.Instance.players.Count == 0) return;
//             Vector3 newPos =
//                 CoreManager.Instance.players[0].transform.position.x <
//                 CoreManager.Instance.players[1].transform.position.x
//                     ? CoreManager.Instance.players[0].transform.position
//                     : CoreManager.Instance.players[1].transform.position;
//
//             float cameraX = mainCamera.transform.position.x;
//
//
//             if (newPos.x >= cameraX)
//             {
//                 Vector3 newCameraPosition = mainCamera.transform.position;
//                 newCameraPosition.x = newPos.x;
//                 mainCamera.transform.position = newCameraPosition;
//             }
//         }
//
//         public void StopMoving()
//         {
//             move = false;
//         }
//
//         public void ContinueMoving()
//         {
//             move = true;
//         }
//     }
// }