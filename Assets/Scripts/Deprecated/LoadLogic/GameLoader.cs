// using System.Collections;
// using SpongeScene.Managers;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace DoubleTrouble.LoadLogic
// {
//     public class GameLoader : MonoBehaviour
//     {
//         [SerializeField] private GameLoaderUI loaderUI;
//
//         private void Start()
//         {
//             StartCoroutine(StartLoadingAsync());
//             loaderUI.Init(100);
//         }
//
//         private IEnumerator StartLoadingAsync()
//         {
//             yield return new WaitForSeconds(0.05f); // fixes rare bugs
//             DontDestroyOnLoad(gameObject);
//             DontDestroyOnLoad(loaderUI.transform.root.gameObject);
//             LoadCoreManager();
//         }
//
//         private void LoadCoreManager()
//         {
//             CoreManager.Instance.Init(loaderUI, OnCoreManagersLoaded);
//         }
//
//         private void OnCoreManagersLoaded()
//         {
//             SceneManager.sceneLoaded += OnLoadComplete;
//             SceneManager.LoadScene("Tutorial");
//         }
//
//         private void OnLoadComplete(Scene scene, LoadSceneMode mode)
//         {
//             SceneManager.sceneLoaded -= OnLoadComplete;
//             Destroy(loaderUI.transform.root.gameObject);
//             Destroy(gameObject);
//         }
//     }
// }