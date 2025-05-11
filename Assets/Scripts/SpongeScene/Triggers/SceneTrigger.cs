using System;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Triggers
{
    public class SceneTrigger : Trigger
    {
        [Header("Trigger Settings")] private int objectsInZone = 0;
        public int requiredObjects = 1; // Number of objects needed to activate the trigger
        private bool isTriggered = false;
        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEndGame);
        }

        private void OnEndGame(object obj)
        {
            objectsInZone = 0;
            isTriggered = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>())
            {
                if(++objectsInZone == requiredObjects)
                {
                    isTriggered = true;
                }
            }
            
    }

        public override bool IsActivated()
        {
            return isTriggered;
        }
    }

}