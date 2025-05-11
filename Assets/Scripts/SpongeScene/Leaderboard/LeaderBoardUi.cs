using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SpongeScene.Leaderboard
{
    public class LeaderboardUI : MonoBehaviour
    {
        public Transform leaderboardParent;
        public GameObject entryPrefab;
        public Vector2 startPosition;
        public float spacing = 40f;
        
        private List<GameObject> instantiatedEntries = new List<GameObject>();

        public void UpdateDisplay(List<LeaderBoard.LeaderboardEntry> entries)
        {
            ClearDisplay();
            
            for (int i = 0; i < entries.Count; i++)
            {
                GameObject newEntry = Instantiate(entryPrefab, leaderboardParent);
                newEntry.transform.localPosition = new Vector2(startPosition.x, startPosition.y - (i * spacing));
                instantiatedEntries.Add(newEntry);
                
                TextMeshProUGUI textComponent = newEntry.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(entries[i].time);
                    string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
                    textComponent.text = $"{i + 1}. {entries[i].name} - {formattedTime} - Deaths: {entries[i].numDeaths}";

                }
            }
        }

        public void ClearDisplay()
        {
            foreach (GameObject entry in instantiatedEntries)
            {
                Destroy(entry);
            }
            instantiatedEntries.Clear();
        }
    }
}