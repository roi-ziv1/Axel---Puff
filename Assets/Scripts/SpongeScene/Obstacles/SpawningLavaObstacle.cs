using System;
using Character;
using SpongeScene.Managers;
using Unity.VisualScripting;

namespace SpongeScene.Obstacles
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SquareSpawner : MonoBehaviour
    {
        [Header("Wave Settings")] public int waves = 5;
        public int squaresPerWave = 5;
        public float squareFadeTime = 2f;
        public float waveInterval = 3f;

        [Header("Square Settings")] public GameObject lavaPrefab;
        public GameObject cautionPrefab;
        public Vector2 squareSpawnRange = new Vector2(10, 10); // X and Y bounds of spawn area

        [Header("Player Settings")] public Transform player;

        private List<GameObject> spawnedLavas = new List<GameObject>(); // Track spawned lava objects
        private List<GameObject> spawnedSquares = new List<GameObject>(); // Track spawned square objects
        private List<Coroutine> runningCoroutines = new List<Coroutine>(); // Track running coroutines

        private void OnEnable()
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
        }

        private void OnDisable()
        {
            CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, ResetObject);
        }

        private void ResetObject(object obj)
        {
            // Stop all coroutines
            foreach (var coroutine in runningCoroutines)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
            }
            runningCoroutines.Clear();

            // Destroy all spawned squares
            foreach (var square in spawnedSquares)
            {
                if (square != null)
                {
                    Destroy(square);
                }
            }
            spawnedSquares.Clear();

            // Destroy all spawned lava objects
            foreach (var lava in spawnedLavas)
            {
                if (lava != null)
                {
                    Destroy(lava);
                }
            }
            spawnedLavas.Clear();

            GetComponent<Collider2D>().isTrigger = true;
        }

        private IEnumerator SpawnWaves()
        {
            for (int wave = 0; wave < waves; wave++)
            {
                Debug.Log($"Wave {wave + 1} starting...");
                bool first = true;
                for (int i = 0; i < squaresPerWave; i++)
                {
                    // Spawn square at a random position
                    Vector2 spawnPosition;
                    if (first)
                    {
                        spawnPosition = CoreManager.Instance.player.transform.position + new Vector3(
                            Random.Range(-0.2f, 0.2f),
                            Random.Range(-0.2f, 0.2f), 0);
                        first = false;
                    }
                    else
                    {
                        spawnPosition = new Vector2(
                            Random.Range(-squareSpawnRange.x, squareSpawnRange.x),
                            Random.Range(-squareSpawnRange.y, squareSpawnRange.y)
                        ) + (Vector2)transform.position;
                    }

                    GameObject square = Instantiate(cautionPrefab, spawnPosition, Quaternion.identity);
                    square.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0); // Start fully transparent
                    spawnedSquares.Add(square);

                    // Start fading coroutine and track it
                    Coroutine fadeCoroutine = StartCoroutine(FadeInSquare(square));
                    runningCoroutines.Add(fadeCoroutine);

                    yield return new WaitForSeconds(0.2f); // Slight delay between spawning squares
                }

                squaresPerWave += 2;

                // Wait before starting the next wave
                yield return new WaitForSeconds(waveInterval);
            }
        }

        private IEnumerator FadeInSquare(GameObject square)
        {
            SpriteRenderer sr = square.GetComponent<SpriteRenderer>();
            float elapsed = 0;

            while (elapsed < squareFadeTime)
            {
                float alpha = Mathf.Lerp(0, 1, elapsed / squareFadeTime);
                sr.color = new Color(1, 1, 1, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Instantiate a lava prefab and add it to the list
            GameObject lava = Instantiate(lavaPrefab, square.transform.position, Quaternion.identity);
            spawnedLavas.Add(lava);

            Destroy(square);
            spawnedSquares.Remove(square); // Remove square from the list
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerManager>() is not null)
            {
                Coroutine waveCoroutine = StartCoroutine(SpawnWaves());
                runningCoroutines.Add(waveCoroutine);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
