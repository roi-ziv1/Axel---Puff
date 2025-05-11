using System;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Triggers
{
    public class NextSceneTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null)
            {
                int nextScene = CoreManager.Instance.SceneManager.LoadNextScene();
            }
        }
    }
}
