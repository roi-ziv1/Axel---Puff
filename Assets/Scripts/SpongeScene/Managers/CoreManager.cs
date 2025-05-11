using System;
using System.Collections;
using System.Collections.Generic;
using Camera;
using Character;
using DoubleTrouble.Managers;
using SpongeScene.Loader;
using SpongeScene.Utilities;
using UnityEngine;

namespace SpongeScene.Managers
{
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance;

        // public GameManager gameManager;
        [SerializeField] private GameObject playerPrefab;
        public PlayerManager player;
        
        // public UIManager uiManager;

        // public SoundManager soundManager;
        
        public EventsManager EventsManager;
        public AdditiveSceneManager SceneManager;
        public PlayerPositionManager PositionManager;
        public SoundManager SoundManager;
        public GameManager GameManager;
        public UIManager UIManager;
        public CameraManager CameraManager;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(new Exception("CoreManager already exists!"));
                return;
            }
        
            Instance = this;

            // Step 1: Initialize EventsManager first
            InitializeEventManager();

            // Step 2: Instantiate the player
            InstantiatePlayer();
        }

        private void InitializeEventManager()
        {
            Debug.Log("EventsManager initialized");
        }

        private void InstantiatePlayer()
        {
            if (player == null)
            {
                GameObject prefab = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                player = prefab.GetComponent<PlayerManager>();
                DontDestroyOnLoad(prefab); // Make sure the player persists across scenes
                Debug.Log("Player instantiated");
                print($"player is {player}");
                print($"player game object is {player.gameObject}");
                player.gameObject.SetActive(false);
            }

        }
        


        // ReSharper disable Unity.PerformanceAnalysis
        public void Init(AdditiveSceneManager sceneManager, Action onComplete)
        {
            SceneManager = sceneManager;
            UIManager.Init();
            PositionManager.Init();
            GameManager.Init();
            SceneManager.Init();
            SoundManager.Init();
            onComplete.Invoke();

        }
    }
}