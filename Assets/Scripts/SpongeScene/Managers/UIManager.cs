using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace SpongeScene.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject waterShotsUI;
        [SerializeField] private GameObject pauseMenu;
        private UiInputAction inputActions;
        

        // [SerializeField] private TextMeshProUGUI livesUI;
        void Awake()
        {
            inputActions = new UiInputAction();
        }

        void OnEnable()
        {
            inputActions.Menu.Enable();
            inputActions.Menu.PauseMenu.performed += OpenPauseMenu;
        }

        private void OpenPauseMenu(InputAction.CallbackContext obj)
        {
            if (CoreManager.Instance.SceneManager.sceneType == SceneType.Menu)return;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void Init()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.StartGame, OnStartGame);

        }

   

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.StartGame, OnStartGame);
            inputActions.Menu.Disable();
            inputActions.Menu.PauseMenu.performed -= OpenPauseMenu;

            


        }
      
        public void UpdateLives(int lives)
        {
            // livesUI.text = lives.ToString();
        }

        private void OnStartGame(object obj)
        {
            // waterShotsUI.SetActive(true);
            // livesUI.gameObject.SetActive(true);
            waterShotsUI.SetActive(true);
            
            // livesUI.gameObject.SetActive(true);
        }
        
        
        
        
    }
}