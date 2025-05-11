using System;
using Character;
using SpongeScene.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace SpongeScene.WaterSource
{
    public class CheckPoint : MonoBehaviour
    {
        private bool reachedThisCheckpoint = false;

        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEnd);
        }
        
        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEnd);
        }

        private void OnEnd(object obj)
        {
            reachedThisCheckpoint = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null)
            {
                if (!reachedThisCheckpoint)
                {
                    // Calculate the center of the collider
                    var component = GetComponent<Collider2D>();
                    if (component != null)
                    {
                        Vector3 center = component.bounds.center;
                        print($"center is {center}");
                        CoreManager.Instance.EventsManager.InvokeEvent(EventNames.ReachedCheckPoint, center);
                    }
                }

                reachedThisCheckpoint = true;
            }
        }
    }
}