// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Deprecated.Managers;
// using DoubleTrouble.Camera;
// using SpongeScene;
// using TMPro;
// using Unity.VisualScripting.ReorderableList;
// using UnityEngine;
// using CoreManager = SpongeScene.Managers.CoreManager;
//
// namespace DoubleTrouble.Managers
// {
//     public class Level : MonoBehaviour
//     {
//         [SerializeField] private List<Wave> levelWaves;
//         [SerializeField] private Trigger levelTrigger;
//         private Renderer arrowRenderer;
//         private int index = 0;
//
//         private void Start()
//         {
//             arrowRenderer = CoreManager.Instance.LevelManager.arrow.GetComponent<Renderer>();
//             DontDestroyOnLoad(gameObject);
//         }
//
//         public IEnumerator StartLevel()
//         {
//             index = 0;
//             // print($"level wave {index} levelwaves total {levelWaves.Count}");
//             CameraManager.Instance.CameraFollower.StopMoving();
//             while (index != levelWaves.Count)
//             {
//                 levelWaves[index].StartWave();
//                 print($"CALL WAVE {index}");
//                 yield return new WaitUntil(() => levelWaves[index].IsWaveOver());
//                 index++;
//             }
//             // add blinking arrow here
//             CameraManager.Instance.CameraFollower.ContinueMoving();
//             yield return StartCoroutine(UtilityFunctions.FadeImage(arrowRenderer, 1, 0, 2f));
//             yield return StartCoroutine(UtilityFunctions.FadeImage(arrowRenderer, 0, 1, 2f));
//
//         }
//
//         public bool IsLevelTriggered()
//         {
//             if (levelTrigger.IsActivated())
//             {
//                 return true;
//             }
//
//             return false;
//
//         }
//     }
// }