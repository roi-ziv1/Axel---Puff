using System;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Triggers
{
    public class EndGameTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            print(other.gameObject.name);
            if (other.gameObject.name == "fish")
            {
                print("Game over called!");
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.GameOver, null);
            }
        }
    }
}