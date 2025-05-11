namespace SpongeScene.Utilities
{
    using UnityEngine;
    
    public class MonoRunner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static void MonoRunnerDontDestroyOnLoad(GameObject obj)
        {
            DontDestroyOnLoad(obj);
        }

        public static GameObject InstantiateObject(GameObject prefab)
        {
            return Instantiate(prefab);
        }
    }
}