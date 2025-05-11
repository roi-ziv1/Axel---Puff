using System.Collections;
using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpongeScene.Menus
{
    public class MainMenu : MonoBehaviour
    {
        // [SerializeField] private MainMenuAnimator mainMenuAnimator;
        [SerializeField] private AudioSource src;
        [SerializeField] private AudioClip press;
        [SerializeField] private AudioClip navigate;
        [SerializeField] private GameObject playButtoncollider;
        [SerializeField] private Image image;
        public Button playButton;
        public Button quitButton;
        private Button[] buttons;
        private int selectedButtonIndex = 0;
        private UiInputAction inputActions;
        private float lastInputTime;
        public float inputDelay = 0.25f; // Adjust the delay as needed
        public float deadZone = 0.5f; // Adjust the dead zone as needed

        private bool isSceneLoading = false; // Flag to check if the scene is loading
        void Awake()
        {
            inputActions = new UiInputAction();
            inputActions.Menu.Navigate.performed += Navigate;
            inputActions.Menu.Submit.performed += Submit;
            buttons = new Button[] { playButton, quitButton }; //otherButton, 

        }


        void OnEnable()
        {
            playButton.onClick.AddListener(OnPlayButton);
            // otherButton.onClick.AddListener(OnOtherButton);
            quitButton.onClick.AddListener(OnQuitButton);
            inputActions.Menu.Enable();
            // mainMenuAnimator.startGame += LoadTutorialScene;
     

        }

        void OnDisable()
        {
            inputActions.Menu.Navigate.performed -= Navigate;
            inputActions.Menu.Submit.performed -= Submit;
            inputActions.Menu.Disable();
            // mainMenuAnimator.startGame -= LoadTutorialScene;
            playButton.onClick.RemoveListener(OnPlayButton);
            quitButton.onClick.RemoveListener(OnQuitButton);
        }

        public void EnableMenu()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
            image.gameObject.SetActive(true);
            image.transform.position = buttons[0].transform.position - Vector3.right * 250;
            buttons[0].Select();

            StartCoroutine(SelectButtonWithDelay(buttons[0]));

        }

        public void DisableMenu()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            image.gameObject.SetActive(false);

        }

        void Navigate(InputAction.CallbackContext obj)
        {
            var direction = obj.ReadValue<Vector2>();
            if (Time.time - lastInputTime < inputDelay)
            {
                return;
            }

            // Check for vertical input with a dead zone
            if (Mathf.Abs(direction.x) > deadZone || Mathf.Abs(direction.y) > deadZone)
            {
                lastInputTime = Time.time;

                if (direction.x > 0 || direction.y < 0)
                {
                    ChangeSelection(1);
                }
                else if (direction.x < 0 || direction.y > 0)
                {
                    ChangeSelection(-1);
                }
            }
        }

        void Submit(InputAction.CallbackContext obj)
        {
            inputActions.Menu.Submit.performed -= Submit;
            inputActions.Menu.Navigate.performed -= Navigate;
            src.PlayOneShot(press);
            if (buttons[selectedButtonIndex] == playButton)
            {
                OnPlayButton();
            }

            else if (buttons[selectedButtonIndex] == quitButton)
            {
                OnQuitButton();
            }
        }

        void ChangeSelection(int direction)
        {
            src.PlayOneShot(navigate);
            // Deselect the current button
            buttons[selectedButtonIndex].OnDeselect(null);
                
            // Update the selected button index
            selectedButtonIndex += direction;

            // Clamp the index to the array bounds using modulo
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = (selectedButtonIndex % buttons.Length + buttons.Length) % buttons.Length;
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
            if (EventSystem.current.currentSelectedGameObject is not null)
            {
                EventSystem.current.SetSelectedGameObject(null); // Deselect any currently selected UI element
            }
            EventSystem.current.SetSelectedGameObject(button.gameObject); // Select the new button
            image.transform.position = button.gameObject.transform.position - Vector3.right *250;
            Debug.Log(button.name);
        }

        void OnPlayButton()
        {
            StartCoroutine(StartGame());
         
        }

        private IEnumerator StartGame()
        {
            CoreManager.Instance.SceneManager.LoadNextScene();
            yield return new WaitForSeconds(0.4f);
            DisableMenu();
            SceneManager.UnloadSceneAsync("Loader");
            yield return new WaitForSeconds(15f);
            CoreManager.Instance.SceneManager.LoadNextScene();
            Time.timeScale = 1f;
            isSceneLoading = true;
            playButtoncollider.SetActive(false);
            // Load the game scene
          
            // CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartGame, null);
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartNewScene, CoreManager.Instance.PositionManager.GetSceneStartingPosition(3));

        }

      
        void OnQuitButton()
        {
            if (isSceneLoading) return; // Prevent further actions if the scene is loading
            Debug.Log("Quit button pressed");
            // Quit the application
            Application.Quit();
        }
    }
}
    
