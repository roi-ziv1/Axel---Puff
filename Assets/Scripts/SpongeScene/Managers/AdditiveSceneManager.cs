using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Update = Unity.VisualScripting.Update;

namespace SpongeScene.Managers
{
    public class AdditiveSceneManager : MonoBehaviour
    {
        [Tooltip("The name of the persistent scene.")] 
        public SceneType sceneType;
        private string persistentSceneName = "PersistentScene";
        private string loaderSceneName = "Loader";

        private int numScenes;

        private int currentSceneIndex = 1; // Tracks the index of the currently active scene

        public void Init()
        {
        }
        
        private void OnDisable()
        {
        }
        private void RestartScene(object obj)
        {
            StartCoroutine(SwitchScene(currentSceneIndex));
        }

        private void Start()
        {
            print("333 ADD EVENT TO SCENE MANAGGER");
            
            DontDestroyOnLoad(gameObject);
            numScenes = SceneManager.sceneCountInBuildSettings;
            // Load the persistent scene if not already loaded
            if (!SceneManager.GetSceneByName(persistentSceneName).isLoaded)
            {
                print("LOAD PERSISTENTSCENE");
                SceneManager.LoadSceneAsync(persistentSceneName, LoadSceneMode.Additive);
                // SceneManager.UnloadSceneAsync(loaderSceneName);
            }
        }

       

        public int LoadNextScene()
        {
            if (currentSceneIndex + 1 < numScenes)
            {
                StartCoroutine(SwitchScene(currentSceneIndex + 1));
                Fader.Instance.FadeOut(1f);
                StartCoroutine(Wait(1f));
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartNewScene, CoreManager.Instance.PositionManager.GetSceneStartingPosition(currentSceneIndex+1));
            

            }
            else
            {
                Debug.LogWarning("No more scenes to load. You are at the last scene.");
            }

            return currentSceneIndex + 1;
        }
        
        private IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
        }
        
        public int ReloadCurrentScene()
        {
            print("RESTART");
            StartCoroutine(SwitchScene(currentSceneIndex));
            return currentSceneIndex;
        }


        public void ReloadMainMenu()
        {
            StartCoroutine(SwitchScene(0));
        }

        public void ReloadLevel()
        {
            StartCoroutine(SwitchScene(2));
        }
        public IEnumerator SwitchScene(int newSceneIndex)
        {
            // Load the new scene additively
            if (newSceneIndex >= 0 && newSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneIndex, LoadSceneMode.Additive);
                while (!loadOperation.isDone)
                {
                    yield return null;
                }

                // Set the new scene as active
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(newSceneIndex));
                sceneType = newSceneIndex switch
                {
                    0 => SceneType.Menu,
                    2 => SceneType.Cutscene,
                    3 => SceneType.Level,
                    4 => SceneType.Cutscene,
                };


            }
            else
            {
                Debug.LogWarning("Invalid scene index: " + newSceneIndex);
                yield break;
            }
            print($"{currentSceneIndex} currrent scene index, {SceneManager.sceneCountInBuildSettings} scene count total");
            // Unload the previous scene
            if (currentSceneIndex > 1 && currentSceneIndex < SceneManager.sceneCountInBuildSettings)
            {   
                print($"unloading scene  {currentSceneIndex}");
                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentSceneIndex);
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }
            }

            // Update the current scene index
            currentSceneIndex = newSceneIndex;
        }


    }

    public enum SceneType
    {
        None=0,
        Cutscene=1,
        Level=2,
        Menu=3,
        
    }
}