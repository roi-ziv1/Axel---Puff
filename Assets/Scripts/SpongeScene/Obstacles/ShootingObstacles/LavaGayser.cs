using System;
using System.Collections;
using Character;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Obstacles.ShootingObstacles
{
    public class LavaGayser : MonoBehaviour
    {
        [SerializeField] Transform gayzerStartPosition;
        [SerializeField] private GameObject LavaDummy; // just until i get particles.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null)
            {
                StartCoroutine(ActivateGayzer());
            }
        }

        private IEnumerator ActivateGayzer()
        {
            float time = 3f;
            // play particles
            LavaDummy.SetActive(true);
            while (time > 0)
            {
                time -= Time.deltaTime;
                RaycastHit2D hit = Physics2D.Raycast(gayzerStartPosition.position, Vector2.down, 30);
                if (hit.collider.gameObject.GetComponent<PlayerManager>() is not null)
                {
                    CoreManager.Instance.EventsManager.InvokeEvent(EventNames.Die, null);
                }

                yield return null;
            }
            LavaDummy.SetActive(false);

            //stop particles
        }
    }
}