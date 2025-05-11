using System;
using System.Collections;
using Character;
using Unity.Cinemachine;
using UnityEngine;

namespace SpongeScene.WaterSource
{
    public class River : MonoBehaviour
    {
        [SerializeField] private float riverSpeed;
        [SerializeField] private float ligeringTime;
        private Coroutine riverCoroutine;
        private void OnTriggerStay2D(Collider2D other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (PlayerIsValidForMoving(player))
            {
                other.transform.position += Vector3.right * riverSpeed * Time.deltaTime;
            }
        }


        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (PlayerIsValidForMoving(player) && gameObject.activeInHierarchy)
            {
                this.StopAndStartCoroutine(ref riverCoroutine, LingeringAffect(other.gameObject));
            }
        }

        private static bool PlayerIsValidForMoving(PlayerManager player)
        {
            return player is not null;
        }

        private IEnumerator LingeringAffect(GameObject gameObject)
        {
            float currentTime = ligeringTime;
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                gameObject.transform.position += Vector3.right * riverSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
