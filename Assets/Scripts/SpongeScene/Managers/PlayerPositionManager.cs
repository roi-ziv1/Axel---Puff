using System;
using System.Collections.Generic;
using DoubleTrouble.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpongeScene.Managers
{
    public class PlayerPositionManager : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private List<Pair> sceneStartingPositions;
        private Vector3 nextRespawnPosition;
        public Vector3 StartPosition => startPosition;


        public void Init()
        {

            CoreManager.Instance.EventsManager.AddListener(EventNames.ReachedCheckPoint, OnReachedCheckPoint);
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
                
        }
        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.ReachedCheckPoint, OnReachedCheckPoint);
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEndGame);
                
        }

        private void OnEndGame(object obj)
        {

            nextRespawnPosition = GetSceneStartingPosition(2);
        }

        private void OnReachedCheckPoint(object obj)
        {
            if (obj is Vector3 pos)
            {
                nextRespawnPosition = pos;
            }
        }

        public Vector3 GetSceneStartingPosition(int scene)
        {
            foreach (var kvp in sceneStartingPositions)
            {
                if (scene == kvp.key)
                {
                    return kvp.value;
                }
            }
            return default;
        }

        public Vector3 GetCheckPointPosition()
        {
            return nextRespawnPosition;
        }
    }

    [Serializable]
    public class Pair
    {
        public int key;
        public Vector3 value;
    }
}