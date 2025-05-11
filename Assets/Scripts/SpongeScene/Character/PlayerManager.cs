using System;
using System.Collections;
using System.Collections.Generic;
using SpongeScene;
using SpongeScene.Managers;
using SpongeScene.Managers.UI;
using SpongeScene.WaterSource;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private float maxWater; // Maximum water the player can hold
        [SerializeField] private float maxMass; // Maximum water the player can hold
        [SerializeField] private float minMass; // Maximum water the player can hold
        [SerializeField] private Vector3 minSize = new Vector3(1f, 1f, 1f); // Minimum size when dry
        [SerializeField] private LayerMask jumpLayers; // Layers considered for grounding

        [SerializeField] private Vector3 maxSize = new Vector3(5.25f, 5.25f, 5.25f); // Maximum size when fully soaked
        [SerializeField] private float respawnDuration = 1f;
        [SerializeField] private Sprite[] maxFishSprites = new Sprite[8];
        [SerializeField] private SpriteRenderer fishSprite;
        [SerializeField] private float waterUsage = 0.1f;

        private Coroutine applyingForceFromAboveCoroutine;
        private WaterUI waterUI;
        private float currentWater;
        private bool isOnStickySurface;
        private Rigidbody2D rb;
        private GroundTypes standingOn;
        private float watersplashcd = 1.8f;
        private float waterSplashedTime;
        private bool canMove;
        private bool inLoadingScreen;
        private float fallingTime;
        public Vector2 ForceFromAbove => forceFromAbove;
        private Vector2 forceFromAbove;
        
        private Vector3 fishMinSize;
        private Vector3 fishMaxSize;
        [SerializeField] private FishState fishState = FishState.Regular;
        [SerializeField] private Animator fishAnimator;
        
        private Sprite[] fishSprites;
        private Sprite[] midFishSprites;
        private Sprite[] minFishSprites;

        private bool loadedNewScene;

        public bool IsOnStickySurface => isOnStickySurface;
        public Rigidbody2D Rb => rb;
        public bool CanMove => canMove;
        public bool InLodingScreen => inLoadingScreen;
        public SpriteRenderer FishSprite => fishSprite;
        public Sprite[] FishSprites => fishSprites;
        public Vector3 FishMinSize => fishMinSize;
        public Vector3 FishMaxSize => fishMaxSize;
        public FishState FishState => fishState;


        private void Awake()
        {
            midFishSprites = new Sprite[maxFishSprites.Length - 2];
            minFishSprites = new Sprite[maxFishSprites.Length - 4];
            for (var i = 0; i < midFishSprites.Length; i++)
            {
                midFishSprites[i] = maxFishSprites[i];
            }
            
            for (var i = 0; i < minFishSprites.Length; i++)
            {
                minFishSprites[i] = maxFishSprites[i];
            }
            fishSprites = minFishSprites;
            rb = GetComponent<Rigidbody2D>();

        }

        private void OnEnable()
        {
            if (CoreManager.Instance != null)
            {
                CoreManager.Instance.EventsManager.AddListener(EventNames.StartNewScene, OnStartNewScene);
                CoreManager.Instance.EventsManager.AddListener(EventNames.StartGame, OnStartGame);
                CoreManager.Instance.EventsManager.AddListener(EventNames.EndGame, OnEndGame);
            }
        }

        private void OnDisable()
        {
            if (CoreManager.Instance != null)
            {
                if (waterUI)
                {
                    waterUI.DisableBars();
                }
                CoreManager.Instance.EventsManager.RemoveListener(EventNames.StartNewScene, OnStartNewScene);
                CoreManager.Instance.EventsManager.RemoveListener(EventNames.StartGame, OnStartGame);
                CoreManager.Instance.EventsManager.RemoveListener(EventNames.EndGame, OnEndGame);
            }
        }

        public void ResetPlayer()
        {
            loadedNewScene = false;

            transform.localScale = minSize;
            currentWater = 0;
            waterUsage = 0.1f;
            fishSprite.sprite = FishSprites[0];
            waterUI.UpdateWater(currentWater);
            fishSprite.transform.localScale = fishMinSize;
            fishState = FishState.Regular;
            fishSprites = minFishSprites;
            gameObject.SetActive(false);
        }
    
        private void OnEndGame(object obj)
        {
           
        }

        private void OnStartGame(object obj)
        {
            loadedNewScene = false;
            gameObject.SetActive(true);
            rb.bodyType = RigidbodyType2D.Dynamic;
            canMove = true;
            Fader.Instance.FadeOut(0.5f);
            Fader.Instance.FadeIn(0.5f);
        }

        public void Die()
        {
            CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Die);
            canMove = false;
            currentWater = maxWater;
            rb.simulated = false;
            waterUI.UpdateWater(currentWater);
            FishSizeCheck();
            StartRespawn();
        }

        private void StartRespawn()
        {
            StartCoroutine(UtilityFunctions.ScaleObjectOverTime(gameObject, minSize, respawnDuration));
            StartCoroutine(UtilityFunctions.MoveObjectOverTime(gameObject, transform.position, transform.rotation,
                CoreManager.Instance.PositionManager.GetCheckPointPosition(), transform.rotation, respawnDuration,
                () =>
                {
                    canMove = true;
                    rb.simulated = true;
                }));
        }

        private void OnStartNewScene(object obj)
        {
            if (obj is Vector3 newPos)
            {
                transform.position = newPos;
                canMove = true;
            }
            fishState = FishState.Regular;
            fishSprites = minFishSprites;
        }


        private void Start()
        {
            if (CoreManager.Instance != null)
            {
                CoreManager.Instance.player = this;
            }

            inLoadingScreen = true;
            currentWater = 0; // Initialize water level
            isOnStickySurface = false;
            waterUI = FindAnyObjectByType<WaterUI>();
            if (waterUI != null)
            {
                waterUI.SetPlayerManager(this);
            }
            fishMinSize = fishSprite.transform.localScale;
            fishMaxSize = fishMinSize * 1.5f;
            
        }
        
        public void SetMaxSize(Vector3 newSize)
        {
            maxSize = newSize;
        }

   
        private IEnumerator DecreaseSizeOverTime(float duration)
        {
            float time = duration;
            float timeLapsed = 0;
            Vector3 startScale = transform.localScale;
            while (timeLapsed < time)
            {
                timeLapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, minSize, timeLapsed / time);
                yield return null;
            }

            transform.localScale = minSize;
        }


        public float CurrentWater => currentWater; // Public getter
        public float MaxWater => maxWater; // Public getter
        public float MaxMass => maxMass; // Public getter
        public float MinMass => minMass; // Public getter
        public Vector3 MinSize => minSize; // Public getter
        public Vector3 MaxSize => maxSize; // Public getter


        private void Update()
        {
            if (transform.position.y > 219 && loadedNewScene == false)
            {
                loadedNewScene = true;
                SceneManager.sceneLoaded += OnEndCutScneLoaded;
                CoreManager.Instance.SceneManager.LoadNextScene();
            }
            if (transform.position.y < DeathZone || Input.GetKeyDown(KeyCode.D))
            {
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.Die, null);
                Die();
            }

            GroundTypes groundType = SetSurfaceType();
            
        }

        private void OnEndCutScneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            gameObject.SetActive(false);
            waterUI.DisableBars();
            CoreManager.Instance.EventsManager.InvokeEvent(EventNames.StartEndCutScene,null);
            
            SceneManager.sceneLoaded -= OnEndCutScneLoaded;
        }

        private static int DeathZone => -30;


        public GroundTypes GetGroundType()
        {
            return standingOn;
        }

        public void UpdateMass()
        {
            // Linearly interpolate mass based on the player's current water level
            float waterRatio = (float)currentWater / maxWater;
            // rb.mass = Mathf.Lerp(minMass, maxMass, waterRatio);
        }

        public bool AddWater(int amount)
        {
            if (currentWater >= maxWater) return false;

            currentWater = Mathf.Clamp(currentWater + amount, 0, maxWater);
            waterUI.UpdateWater(currentWater);
            return true; // Successfully added water
        }

        public bool UseWater(float amount)
        {
            if (currentWater <= 0) return false;
            print($"current water : {currentWater}");
            currentWater -= waterUsage;
            waterUI.UpdateWater(currentWater);
            return true; // Successfully used water
        }

        public bool CanAbsorb()
        {
            return !Mathf.Approximately(currentWater, maxWater);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("StickySurface"))
            {
                standingOn = GroundTypes.StickySurface;
            }
        }


        public bool IsOnGround()
        {
            return standingOn != GroundTypes.None;
        }

        public bool IsOnSurface()
        {
            return Physics2D.CircleCast(transform.position, 1f, Vector2.down, 1f, jumpLayers);
        }

        public GroundTypes SetSurfaceType()
        {
            // Perform a CircleCastAll downward from the player's position
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.1f, Vector2.down, 0.001f, jumpLayers);

            if (hits.Length > 0)
            {
                RaycastHit2D closestHit = hits[0];
                float closestDistance = closestHit.distance;
                
                // Find the closest hit
                foreach (var hit in hits)
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) // need to fix this
                    {
                        closestHit = hit;
                        break;
                    }

                    if (hit.distance < closestDistance)
                    {
                        closestHit = hit;
                        closestDistance = hit.distance;
                    }
                }

                if (closestHit.collider.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    standingOn = GroundTypes.Water;
                }
                else if (closestHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                         closestHit.collider.gameObject.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    standingOn = GroundTypes.Ground;
                }
                else if (closestHit.collider.gameObject.layer == LayerMask.NameToLayer("StickySurface"))
                {
                    standingOn = GroundTypes.StickySurface;
                }
            }
            else
            {
                // Default to Ground if no valid surface is detected
                standingOn = GroundTypes.None;
            }

            return standingOn;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Lava"))
            {
                CoreManager.Instance.EventsManager.InvokeEvent(EventNames.Die, null);
                Die();
            }
            
            else if (other.gameObject.CompareTag("Token"))
            {
                CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.Upgrade);
                if (other.gameObject.name == "Upgrade1")
                {
                    // maxWater += 4;
                    waterUsage = 0.08f;
                    Destroy(other.gameObject);

                    fishState = FishState.Upgrade1;
                    fishSprites = midFishSprites;
                    currentWater = maxWater;
                    fishSprite.transform.localScale = maxSize;
                    
                    waterUI.UpdateWater(currentWater);
                    waterUI.ChangeWaterBar(1);
                    FishSizeCheck();
                    return;
                }

                // maxWater = 30;
                waterUsage = 0.03f;
                Destroy(other.gameObject);

                fishState = FishState.Upgrade2;
                fishSprites = maxFishSprites;
                // change the fish size to be the percent of the way between min and max size
                currentWater = maxWater;
                fishSprite.transform.localScale = maxSize;
                
                waterUI.UpdateWater(currentWater);
                waterUI.ChangeWaterBar(2);
                FishSizeCheck();
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Water") &&
                Time.time > waterSplashedTime + watersplashcd)
            {
                CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.WaterSplash);
                waterSplashedTime = Time.time;
            }

            else if (other.gameObject.layer == LayerMask.NameToLayer("Token"))
            {
                AddWater(2);
                // lerp the fish size to be the percent of the max water 
                fishSprite.transform.localScale = Vector3.Lerp(fishMinSize, fishMaxSize, currentWater / maxWater);
                other.gameObject.GetComponent<WaterToken>().Activate();
            }
        }
        
        public void FishSizeCheck()
        {
            var waterPercentage = CurrentWater / MaxWater;
            fishSprite.transform.localScale = Vector3.Lerp(fishSprite.transform.localScale, MaxSize, waterPercentage);
            // fishSprite.sprite = waterPercentage switch
            // {
            //     < 0.2f => FishSprites[0],
            //     < 0.4f => FishSprites[1],
            //     < 0.6f => FishSprites[2],
            //     < 0.8f => FishSprites[3],
            //     < 1f => FishSprites[4],
            //     _ => FishSprites[5]
            // };
            // change the sprite according to the percentage threshold and the length of the fishSprites array
            int spriteIndex = Mathf.FloorToInt(waterPercentage * (FishSprites.Length - 1));
            if (spriteIndex < 0)
            {
                spriteIndex = 0;
            }
            fishSprite.sprite = FishSprites[spriteIndex];
            ChangeFishAnimation((spriteIndex + 1).ToString());
        }


        public void ApplyForceFromAbove(Vector2 force)
        {
            this.StopAndStartCoroutine(ref applyingForceFromAboveCoroutine, ApplyForceFromAboveForDuration(force));
        }

        private IEnumerator ApplyForceFromAboveForDuration(Vector2 force)
        {
            forceFromAbove = force;
            yield return new WaitUntil(IsOnGround);
            yield return new WaitForSeconds(0.03f);
            forceFromAbove = default;
        }
        
        public void ChangeFishAnimation(string trigger)
        {
            fishAnimator.SetTrigger(trigger);
        }


  
    }

    public enum GroundTypes
    {
        None = 0,
        Ground = 1,
        Water = 2,
        StickySurface = 3,
        River = 4,
    }

    public enum FishState
    {
        Regular,
        Upgrade1,
        Upgrade2,
        
    }
}