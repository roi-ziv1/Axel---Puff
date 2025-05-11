using System.Collections.Generic;
using System.Numerics;
using DoubleTrouble.Utilities;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace DoubleTrouble.Managers
{
    public class Wave : MonoBehaviour
    {
        [SerializeField] private List<SerializableTuple<Vector3, Enemy>> waveEnemies;
        
        public void StartWave()
        {
            foreach (var kvp in waveEnemies)
            {
                kvp.second.transform.position = kvp.first;
                kvp.second.gameObject.SetActive(true);
                kvp.second.IsAlive = true;
            }
        }
        public bool IsWaveOver()   // bad performance
        {
            foreach (var kvp in waveEnemies)
            {
                if (kvp.second.gameObject.activeInHierarchy)
                {
                    print(kvp.second.gameObject.name);
                    return false;
                }
                print(kvp.second.gameObject.name);
                print(kvp.second.IsAlive);
                print("---");
            }
            print("all enemies are dead go next");
            return true;
        }
        
    }
}