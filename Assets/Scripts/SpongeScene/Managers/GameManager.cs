using System;
using UnityEngine;

namespace SpongeScene.Managers
{
    public class GameManager : MonoBehaviour
    {

        private float startTime;
        private bool isGameRunning = false;
        private int deathCount = 0;
        public string playerName;
        public string playerTimeString;
        public float playerTimeFloat;
        
        

        public void Init()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.StartTimer, ResetStats);
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, OnPlayerDeath);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.StartTimer, ResetStats);
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, OnPlayerDeath);
     
        }

        private void ResetStats(object o)
        {
            startTime = Time.time;
            isGameRunning = true;
            deathCount = 0; // Reset deaths at the start of a new game
        }

        private void OnPlayerDeath(object o)
        {
            deathCount++;
        }

        public string GetTimeSinceStart()
        {
            float elapsedTime = isGameRunning ? Time.time - startTime : 0f;
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            
            // Format time as mm:ss
            playerTimeString = timeSpan.ToString(@"mm\:ss");
            playerTimeFloat = elapsedTime;
            return playerTimeString;
        }

        public int GetDeathCount()
        {
            return deathCount;
        }


        public void SetPlayerName(string input)
        {
            playerName = input;
        }
    }
}