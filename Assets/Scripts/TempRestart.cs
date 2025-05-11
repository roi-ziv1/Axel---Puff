using UnityEngine;

public class TempRestart : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        // Check if R was pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
