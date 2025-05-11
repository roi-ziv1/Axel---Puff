using System;
using System.Collections.Generic;
using SpongeScene.Leaderboard;
using SpongeScene.Managers;
using TMPro;
using UnityEngine;

namespace SpongeScene
{
    public class LeaderBoard : MonoBehaviour
    {
        [Serializable]
        public struct LeaderboardEntry
        {
            public string name;
            public float time;
            public int numDeaths;
        }

        [SerializeField] private LeaderboardUI leaderboardUI;
        private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        private const int MaxEntries = 10;
        private float deleteKeyHoldTime = 0f;
        private const float deleteThreshold = 6f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.D))
            {
                deleteKeyHoldTime += Time.deltaTime;
                if (deleteKeyHoldTime >= deleteThreshold)
                {
                    RemoveData();
                    deleteKeyHoldTime = 0f;
                }
            }
            else
            {
                deleteKeyHoldTime = 0f;
            }
        }

        private void Start()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.ToMainMenu, DeactivateLeaderBoard);
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, DeactivateLeaderBoard);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.ToMainMenu, DeactivateLeaderBoard);
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, DeactivateLeaderBoard);
        }
        
        public void ActivateLeaderBoard()
        {
            leaderboardUI.UpdateDisplay(entries);
        }

        private void DeactivateLeaderBoard(object o)
        {
            leaderboardUI.ClearDisplay();
        }

        public void RemoveData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            entries.Clear();
            leaderboardUI.ClearDisplay();
            Debug.Log("Leaderboard data cleared.");
        }

        public void AddEntry(string playerName, float playerTime, int playerDeaths)
        {
            entries.Add(new LeaderboardEntry { name = playerName, time = playerTime, numDeaths = playerDeaths });
            entries.Sort((a, b) => a.time.CompareTo(b.time));
            if (entries.Count > MaxEntries)
            {
                entries.RemoveAt(entries.Count - 1);
            }
            SaveLeaderboard();
        }

        private void SaveLeaderboard()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                PlayerPrefs.SetString($"Leaderboard_Name_{i}", entries[i].name);
                PlayerPrefs.SetFloat($"Leaderboard_Time_{i}", entries[i].time);
                PlayerPrefs.SetInt($"Leaderboard_Deaths_{i}", entries[i].numDeaths);
            }
            PlayerPrefs.SetInt("Leaderboard_Count", entries.Count);
            PlayerPrefs.Save();
        }

        public void LoadLeaderboard()
        {
            entries.Clear();
            int count = PlayerPrefs.GetInt("Leaderboard_Count", 0);
            for (int i = 0; i < count; i++)
            {
                string name = PlayerPrefs.GetString($"Leaderboard_Name_{i}", "");
                float time = PlayerPrefs.GetFloat($"Leaderboard_Time_{i}", 0);
                int deaths = PlayerPrefs.GetInt($"Leaderboard_Deaths_{i}", 0);
                
                if (!string.IsNullOrEmpty(name))
                {
                    entries.Add(new LeaderboardEntry { name = name, time = time, numDeaths = deaths });
                }
            }
            entries.Sort((a, b) => a.time.CompareTo(b.time));
        }
    }
}
