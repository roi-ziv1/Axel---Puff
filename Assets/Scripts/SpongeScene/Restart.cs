using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpongeScene
{
    public class Restart : MonoBehaviour
    {
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.R)) return;
            int currScene = SceneManager.GetActiveScene().buildIndex;
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartNewScene, CoreManager.Instance.PositionManager.GetSceneStartingPosition(currScene));
            // CoreManager.Instance.SceneManager.ReloadCurrentScene();
            // Reload the current scene without unloading other scenes to avoid losing the player
            SceneManager.UnloadSceneAsync(currScene).completed += (operation) =>
            {
                // Reload the scene additively
                SceneManager.LoadScene(currScene, LoadSceneMode.Additive);
            };
            
        }
    }
}
