using System.Collections;
using System.Collections.Generic;
using DoubleTrouble.Utilities;
using SpongeScene.Managers;
using SpongeScene.Menus;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpongeScene.Loader
{
    public class GameLoader : MonoBehaviour
    {
        // [SerializeField] private GameLoaderUI loaderUI;
        
        [SerializeField] private AdditiveSceneManager sceneManager;
        [SerializeField] private List<SerializableTuple<GameObject, Button>> buttons;
        [SerializeField] private MainMenu mainMenu;

        private Animator animator;
        
        private void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(StartLoadingAsync());
            animator.SetTrigger("Start");
            // loaderUI.Init(100);
        }

        private IEnumerator StartLoadingAsync()
        {
            yield return new WaitForSeconds(0.05f); // fixes rare bugs
            LoadCoreManager();
        }

        private void LoadCoreManager()
        {
            CoreManager.Instance.Init(sceneManager, OnCoreManagersLoaded);
        }

        private void OnCoreManagersLoaded()
        {
            StartCoroutine(FakeLoad());
        }

        private IEnumerator FakeLoad()
        {
            yield return new WaitForSeconds(3.7f);
            animator.SetTrigger("Stop"); // Ensure you have a "Stop" trigger in your Animator

            mainMenu.EnableMenu();

            // while (loaderUI._progress < 100)
            // {
            //     loaderUI.AddProgress(1);
            //     if (loaderUI._progress == 90)
            //     {
            //
            //     }
            //     yield return new WaitForSeconds(0.01f);
            //     
            // }

            // loaderUI.DestroyUI();
            // Destroy(gameObject);
        }

     
  
    }
}