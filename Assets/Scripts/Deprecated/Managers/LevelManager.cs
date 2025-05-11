// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Deprecated.Managers;
// using DoubleTrouble.Utilities;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using CoreManager = SpongeScene.Managers.CoreManager;
//
// namespace DoubleTrouble.Managers
// {
//     public class LevelManager : MonoBehaviour
//     {
//         [SerializeField] private List<Level> levels;
//         private int currentLevel;
//         public GameObject arrow;
//
//         private void Start()
//         {
//             CoreManager.Instance.LevelManager = this;
//             DontDestroyOnLoad(gameObject);
//             StartCoroutine(HandleLevels());
//         }
//
//         public IEnumerator HandleLevels()
//         {
//             while (currentLevel < levels.Count)
//             {
//                 if (levels[currentLevel].IsLevelTriggered())
//                 {
//                     StartCoroutine(levels[currentLevel].StartLevel()); // needs to be a coroutine
//                     currentLevel++;
//                 }
//
//                 yield return new WaitForSeconds(0.7f);
//             }
//         }
//         
//     }
// }