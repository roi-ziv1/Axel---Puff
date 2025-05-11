using System;
using System.Collections;
using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SpongeScene.Menus
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private AudioSource src;
        [SerializeField] private AudioClip press;
        [SerializeField] private AudioClip navigate;
        public Button mainMenuButton;
        public Button restartButton;
        private Button[] buttons;
        private int selectedButtonIndex = 0;
        private UiInputAction inputActions;
        private DateTime lastInputTime;
        public float inputDelay = 0.15f; // Adjust the delay as needed
        public float deadZone = 0.5f; // Adjust the dead zone as needed

        private bool isSceneLoading = false; // Flag to check if the scene is loading
        void Awake()
        {
            inputActions = new UiInputAction();
      
            buttons = new Button[] {restartButton, mainMenuButton}; //otherButton, 
        }

        void OnEnable()
        {
            inputActions.Menu.Navigate.performed += Navigate;
            inputActions.Menu.Submit.performed += Submit;
            restartButton.onClick.AddListener(OnRetart);
            mainMenuButton.onClick.AddListener(OnMainMenu);
            inputActions.Menu.Enable();
            // mainMenuAnimator.startGame += LoadTutorialScene;
            buttons[0].Select();
            selectedButtonIndex = 0;
        }

       
        void OnDisable()
        {
            inputActions.Menu.Navigate.performed -= Navigate;
            inputActions.Menu.Submit.performed -= Submit;
            inputActions.Menu.Disable();
            // mainMenuAnimator.startGame -= LoadTutorialScene;
            mainMenuButton.onClick.RemoveListener(OnMainMenu);
            restartButton.onClick.RemoveListener(OnRetart);

        }

        void Navigate(InputAction.CallbackContext obj)
        {
            var direction = obj.ReadValue<Vector2>();
            print("start navigate 333");
            if ((DateTime.UtcNow - lastInputTime).TotalSeconds < inputDelay)
            {
                print($"cancel navigate because {(DateTime.UtcNow - lastInputTime).TotalSeconds} is less than {inputDelay}333");
                return;
            }

            print("navigate 333");
            // Check for vertical input with a dead zone
            if (Mathf.Abs(direction.x) > deadZone || Mathf.Abs(direction.y) > deadZone)
            {
                lastInputTime = DateTime.UtcNow;

                if (direction.x > 0 || direction.y < 0)
                {
                    ChangeSelection(1);
                    print("change selection 333");
                }
                else if (direction.x < 0 || direction.y > 0)
                {
                    ChangeSelection(-1);
                    print("change selection 333");

                }
            }
        }

        void Submit(InputAction.CallbackContext obj)
        {
            inputActions.Menu.Submit.performed -= Submit;
            inputActions.Menu.Navigate.performed -= Navigate;
            src.PlayOneShot(press);
            print($"selected index in {selectedButtonIndex} 123");
       
             if (buttons[selectedButtonIndex] == mainMenuButton)
            {
                OnMainMenu();
            }

            else
            {
                OnRetart();
            }
        }

        void ChangeSelection(int direction)
        {
            src.PlayOneShot(navigate);
            // Deselect the current button
            buttons[selectedButtonIndex].OnDeselect(null);

            // Update the selected button index
            selectedButtonIndex += direction;
            print($"chaning selection to {selectedButtonIndex}");
            // Clamp the index to the array bounds using modulo
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = buttons.Length - 1;
            }
            else
            {
                selectedButtonIndex = selectedButtonIndex % buttons.Length;
            }

            print(selectedButtonIndex);

            // Select the new button using EventSystem
            StartCoroutine(SelectButtonWithDelay(buttons[selectedButtonIndex]));
        }

        private IEnumerator SelectButtonWithDelay(Button button)
        {
            // Briefly wait to ensure smooth transition
            yield return null; // Wait for one frame
            EventSystem.current.SetSelectedGameObject(null); // Deselect any currently selected UI element
            EventSystem.current.SetSelectedGameObject(button.gameObject); // Select the new button
            Debug.Log(button.name);
        }

        private void OnResume()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            print("RESUME 22");
        }
        
        void OnMainMenu()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.sceneLoaded += OnManMenu;
            CoreManager.Instance.SceneManager.ReloadMainMenu();
            // CoreManager.Instance.player.EnterLoadingScreenMode();
         


        }

        private void OnManMenu(Scene arg0, LoadSceneMode arg1)
        {
            CoreManager.Instance.CameraManager.MainCamera.SetActive(false);
            CoreManager.Instance.player.ResetPlayer();
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.ToMainMenu, null);
            SceneManager.sceneLoaded -= OnManMenu;

        }

        private void OnRetart()
        {
            Time.timeScale = 1f;
            CoreManager.Instance.player.transform.position =
                CoreManager.Instance.PositionManager.GetSceneStartingPosition(3);
             CoreManager.Instance.EventsManager.InvokeEvent(EventNames.EndGame, null);
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.Die, null);
            CoreManager.Instance.player.ResetPlayer();
            CoreManager.Instance.SceneManager.ReloadLevel();
           
            gameObject.SetActive(false);

        }
    }
    
}
    
