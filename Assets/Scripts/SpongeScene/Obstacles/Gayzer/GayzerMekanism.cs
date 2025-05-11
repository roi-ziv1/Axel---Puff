using System;
using System.Collections;
using Character;
using SpongeScene.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpongeScene.WaterTriggers
{
    public class GayzerMekanism : MonoBehaviour
    {
        [SerializeField] private GameObject gayzer;
        [SerializeField] private GameObject gayserEruptPoint;
        [SerializeField] private ParticleSystem gayzerParticles;
        [SerializeField] private ParticleSystem smokeParticles;
        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;
        [SerializeField] private Transform gayzerSpawnPosition;
        [SerializeField] private float forceStrength;
        [SerializeField] private Animator animator;
        [SerializeField] private Vector2 waterDir;
        [SerializeField] private float gayzerDelay;
        private Coroutine gayzerCoroutine;
        private Coroutine boilingCoroutine;
        private SpriteRenderer _renderer;
        private float raycastRadius = 4;
        private int currentHits = 0;

        private ParticleSystem.MainModule gayzerMainModule;

        private void Start()
        {
            _renderer = gayzer.GetComponent<SpriteRenderer>();
            gayzerParticles.Stop();
            smokeParticles.Stop();

            // Cache the particle system's main module
            gayzerMainModule = gayzerParticles.main;
        }

        private void OnParticleCollision(GameObject other)
        {
          
            print("GAYZER HIT!");
            if (gayzerCoroutine is null)
            {
                print("START GAYZER");
                gayzerCoroutine = StartCoroutine(StateGayzer());
            }
        }

        private IEnumerator StateGayzer()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("Start");
            print("STARTED!");
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.BoilingWater);
            smokeParticles.Play();
            yield return new WaitForSeconds(2f);
            StartCoroutine(UtilityFunctions.ShakeObject(gayserEruptPoint.transform,3f,0.05f));
            yield return new WaitForSeconds(gayzerDelay - 2f);
            boilingCoroutine = null;
        }

        public void ShootGayzer()
        {
            smokeParticles.Stop();
            // AdjustParticleDistance(); // Adjust particles to match currentHits
            gayzerParticles.Play();
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Gayser);
            StartCoroutine(StopWaterAfterDelay());
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                gayzerSpawnPosition.position,
                raycastRadius,
                waterDir,
                30f
            );
            print($"--hit {hits.Length} targets with gayzer!");
            foreach (var hit in hits)
            {
                print("--" + hit.collider.gameObject.name);
                if (hit.collider.GetComponent<PlayerManager>() != null)
                {
                    Rigidbody2D playerRigidbody = hit.collider.GetComponent<Rigidbody2D>();
                    if (playerRigidbody != null)
                    {
                        
                        playerRigidbody.AddForce(waterDir*forceStrength);
                    }
                }
            }

            currentHits = 0;
            gayzerCoroutine = null;
        }

        private void AdjustParticleDistance()
        {
            // Set particle start speed or lifetime proportional to currentHits
            // float distanceMultiplier = Mathf.Clamp((float)currentHits / maxHits, 0.1f, 1f);
            // gayzerMainModule.startSpeed = 40f * distanceMultiplier; // Adjust speed range
            // gayzerMainModule.startLifetime = 2f * distanceMultiplier; // Adjust lifetime range
        }

        private IEnumerator StopWaterAfterDelay()
        {
            yield return new WaitForSeconds(1.5f);
            gayzerParticles.Stop();
        }
    }
}
