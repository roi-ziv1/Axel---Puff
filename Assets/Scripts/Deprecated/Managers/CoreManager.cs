// using System;
// using System.Collections.Generic;
// using DoubleTrouble.LoadLogic;
// using DoubleTrouble.Managers;
// using DoubleTrouble.Utilities;
// using UnityEngine;
//
// namespace Deprecated.Managers
// {
//     public class CoreManager : MonoBehaviour
//     {
//         public static CoreManager Instance;
//
//         // public GameManager gameManager;
//
//         // public Player player;
//         
//         // public UIManager uiManager;
//
//         // public SoundManager soundManager;
//         
//         public EventsManager EventsManager;
//         public LevelManager LevelManager;
//         public GameManager GameManager;
//         public MonoRunner MonoRunner;
//         public List<Player> players;
//
//         private void Awake()
//         {
//             if (Instance != null)
//             {
//                 Debug.Log(new Exception("CoreManager already exists!"));
//                 DontDestroyOnLoad(gameObject);
//                 return;
//             }
//             Instance = this;
//         }
//
//         public void Init(GameLoaderUI loaderUI, Action onComplete)
//         {
//             MonoRunner = new GameObject("CoreManagerRunner").AddComponent<MonoRunner>();
//             EventsManager = new EventsManager();
//             GameManager = new GameManager();
//
//             players = new List<Player>();
//             loaderUI.AddProgress(100);
//             onComplete.Invoke();
//
//         }
//     }
// }