using System;
using System.Collections;
using System.Collections.Generic;
using SpongeScene;
using SpongeScene.Managers;
using UnityEngine;

namespace Character
{
    public class AbsorbWater : MonoBehaviour
    {
        [SerializeField] private GameObject hasWaterEffect;
        [SerializeField] private AudioSource src;
        [SerializeField] private List<AudioClip> absorbClips;
        [SerializeField] private AudioClip finishAbsorbing;
        [SerializeField] private ParticleSystem p;
        [Header("Mass Settings")] [SerializeField]
        private float minMass;

        [SerializeField] private float maxMass;

        [SerializeField] private float maxAbsorbTime;

        private SpriteRenderer fishSprite;

        private CapsuleCollider2D col;
        // An array of sprites to represent the different sizes of the character


        private Rigidbody2D rb;
        private PlayerManager player;
        private SpongeMovement spongeMovement;
        private bool canAbsorb = false;
        private bool absorbing = false;
        public Vector3 fishTargetSize;

        private ReduceWater waterSource;
        private Dictionary<FishState, float> stateToAbsorbDurationMap;
        private Dictionary<FishState, AudioClip> stateToAbsorbClipMap;

        private void Awake()
        {
            maxAbsorbTime = 3f;
            spongeMovement = GetComponent<SpongeMovement>();
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();
            col = GetComponent<CapsuleCollider2D>();
            stateToAbsorbDurationMap = new Dictionary<FishState, float>
            {
                { FishState.Regular, 1.5f },
                { FishState.Upgrade1, 2.5f },
                { FishState.Upgrade2, 3.5f }
            };
            stateToAbsorbClipMap = new Dictionary<FishState, AudioClip>
            {
                { FishState.Regular, absorbClips[0]},
                { FishState.Upgrade1, absorbClips[1] },
                { FishState.Upgrade2, absorbClips[2] }
            };
            
        }

        private void Start()
        {
            fishSprite = player.FishSprite;
            fishSprite.sprite = player.FishSprites[0];
            fishTargetSize = fishSprite.transform.localScale * 1.5f;
            player.SetMaxSize(fishTargetSize);
            Debug.Log("Fish target size: " + fishTargetSize);
            p.Stop();
            CoreManager.Instance.EventsManager.AddListener(EventNames.StartGame, OnStart);

        }

        private void OnStart(object obj)
        {
            p.Stop();
        }


        void Update()
        {
            if (player.CanAbsorb())
            {
                TryToAbsorb();
            }

            if (waterSource is not null)
            {
                // if (waterSource.GetIfEmpty())
                // {
                //     StopAllCoroutines();
                // }
            }
            
        }
        void TryToAbsorb()
        {
            // UserInput.instance.controls.Movement.Absorb.WasPressedThisFrame()
            if (canAbsorb)
            {
                print("trying to absorb");
                absorbing = true;
                canAbsorb = false;
                float distance =5f;
                Vector2 origin = transform.position;
                float radius = col.size.x / 2f;
            
                // Perform CircleCast
                RaycastHit2D hit = Physics2D.CircleCast(
                    origin,
                    radius,
                    Vector2.down,
                    distance,
                    LayerMask.GetMask("Water")
                );

                // Draw the CircleCast ray
                Debug.DrawRay(origin, Vector2.down * distance, Color.blue, 0.1f);
                Debug.DrawLine(origin + Vector2.right * radius, origin + Vector2.right * radius + Vector2.down * distance, Color.green, 0.1f);
                Debug.DrawLine(origin - Vector2.right * radius, origin - Vector2.right * radius + Vector2.down * distance, Color.green, 0.1f);

                if (hit.collider != null)
                {
                    if (waterSource.GetIfEmpty())
                    {
                        // Stop the coroutines if the water is empty
                        StopAllCoroutines();
                        return;
                    }
                    StartCoroutine(ChangeFishSizeOverTime(fishTargetSize)); // Gradually increase size
                    StartCoroutine(AddWaterOverTime());
                }
                else
                {
                    // print("no water was found");
                }
            }
        }


        private IEnumerator AddWaterOverTime()
        {
            src.clip = stateToAbsorbClipMap[player.FishState];
            src.Play();
            p.Play();
            // UserInput.instance.controls.Movement.Absorb.IsPressed()
            while (absorbing && player.AddWater(1))
            {
                
                if (waterSource.GetIfEmpty())
                {
                    break;
                }
                player.UpdateMass();
                waterSource.Reduce();
                if (!player.CanAbsorb()) break;
                yield return new WaitForSeconds(stateToAbsorbDurationMap[player.FishState]/ (player.MaxWater));
            }

            src.clip = null;
            src.Stop();
            p.Stop();
            if (player.FishState == FishState.Upgrade2)
            {
                src.clip = finishAbsorbing;
                src.Play();
            }
        }

        private IEnumerator ChangeSizeOverTime(Vector3 targetSize)
        {
            float absorbTime = (player.MaxWater - player.CurrentWater) / maxAbsorbTime;
            float elapsedTime = 0f; // Tracks elapsed time
            Vector3 initialSize = transform.localScale; // Starting size

            while (elapsedTime < maxAbsorbTime && UserInput.instance.controls.Movement.Absorb.IsPressed())
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / absorbTime); // Progress from 0 to 1
                transform.localScale = Vector3.Lerp(initialSize, targetSize, progress);
                yield return null;
            }
        }

        private IEnumerator ChangeFishSizeOverTime(Vector3 targetSize)
        {
            print("changing fish size");
            float absorbTime = (player.MaxWater - player.CurrentWater) / maxAbsorbTime;
            float elapsedTime = 0f; // Tracks elapsed time
            Vector3 initialSize = fishSprite.transform.localScale; // Starting size
            //UserInput.instance.controls.Movement.Absorb.IsPressed()
            while (elapsedTime < maxAbsorbTime && absorbing)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / absorbTime); // Progress from 0 to 1
                fishSprite.transform.localScale = Vector3.Lerp(initialSize, targetSize, progress);
                float waterPercentage = player.CurrentWater / player.MaxWater;
                // change sprite according to percentage threshold
                // fishSprite.sprite = waterPercentage switch
                // {
                //     < 0.2f => player.FishSprites[0],
                //     < 0.4f => player.FishSprites[1],
                //     < 0.6f => player.FishSprites[2],
                //     < 0.8f => player.FishSprites[3],
                //     < 1f => player.FishSprites[4],
                //     _ => player.FishSprites[5]
                // };
                int spriteIndex = Mathf.FloorToInt(waterPercentage * (player.FishSprites.Length - 1));
                if (spriteIndex < 0)
                {
                    spriteIndex = 0;
                }
                fishSprite.sprite = player.FishSprites[spriteIndex];
                player.ChangeFishAnimation((spriteIndex + 1).ToString());
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                ReduceWater water = other.GetComponent<ReduceWater>();
                if (water != null)
                {
                    waterSource = water;
                    canAbsorb = true;
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Token"))
            {
                // change fish size to reflect the current water percentage
                player.FishSizeCheck();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                canAbsorb = false;
                absorbing = false;
            }
        }

        
    }
}