using System.Collections;
using SpongeScene;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Character
{
    public class ShootWater : MonoBehaviour
    {
        [SerializeField] private GameObject waterPrefab;
        [SerializeField] private Transform waterSpawnPoint;
        [SerializeField] private float shootCooldown = 0.5f;
        [SerializeField] private float horizontalForce = 10f; // Force applied horizontally
        [SerializeField] private float verticalForce = 8f; // Force applied vertically
        [SerializeField] private float diagonalHorizontalBias = 1.5f; // Boost for horizontal force on diagonal
        [SerializeField] private float diagonalVerticalBias = 0.5f; // Reduction for vertical force on diagonal
        [SerializeField] private float downShotYValueThreshold = -0.3f;
        [SerializeField] private float lowVelocityThreshold = 2f; // Threshold for low velocity
        [SerializeField] private float forceIncreaseMultiplierX = 1.5f; // Multiplier to increase force if velocity is low
        [SerializeField] private float forceIncreaseMultiplierY = 1.5f; // Multiplier to increase force if velocity is low
        [SerializeField] private float reduceXRecoil = 0.2f; // Multiplier to decreease force if side hoot is triggered
        [SerializeField] private float sideShootThreshold = 0.3f; // Multiplier to increase force if velocity is low
        [SerializeField] private ParticleSystem splashEffect;
        [SerializeField] private ParticleSystem waterTrail;
        [SerializeField] private AudioSource src;
        [SerializeField] private AudioClip splashSound;
        [SerializeField] private ParticleSystem waterHose;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer playerSprite;
        private SpriteRenderer fishSprite;
        private PlayerManager player;
        private bool onCooldown = false;
        [SerializeField] private float spawnDistance;
        private SpriteRenderer spawnPointSpriteRenderer;
        private Vector2 defaultSpawnPointPosition;
        private AbsorbWater absorbWater;
        private float defaultSpawnPointRotation;
        private Rigidbody2D rb;
        private Vector2 currentShotDirection;
        private Vector3 sizeDecreasePerShot;
        private SpongeMovement spongeMovement;
        private float upwardsShotThreshold = -0.85f;

        [SerializeField] private float aimAngle;
        private static readonly int Jet = Animator.StringToHash("Jet");


        void Start()
        {
            // spawnPointSpriteRenderer = waterSpawnPoint.GetComponent<SpriteRenderer>();
            defaultSpawnPointPosition = waterSpawnPoint.localPosition;
            // absorbWater = GetComponent<AbsorbWater>();
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();
            sizeDecreasePerShot = (player.MaxSize - player.MinSize) / player.MaxWater;
            spongeMovement = GetComponent<SpongeMovement>();
            // waterTrail.Stop();
            waterHose.Stop();
            fishSprite = player.FishSprite;
            src.clip = splashSound;
        }

        void Update()
        {
            Aim();
        }

        private void FixedUpdate()
        {
            if (UserInput.instance.controls.Movement.Shoot.IsPressed() && !onCooldown)
            {
                if (player.UseWater(1))
                {
                    Shoot();
                }
            }
            if (UserInput.instance.controls.Movement.Hose.IsPressed())
            {
                if (player.UseWater(0.1f))
                {
                    waterHose.Play();
                    anim.SetBool(Jet, true);
                    Shoot(true);

                    if (!src.isPlaying)  // Start playing if not already playing
                    {
                        src.Play();
                        StartCoroutine(LoopWaterSound());
                    }
                }
                else
                {
                    StopWaterEffects();
                }
            }
            else if (!UserInput.instance.controls.Movement.Hose.IsPressed())
            {
                StopWaterEffects();
            }
        }

        private void StopWaterEffects()
        {
            waterHose.Stop();
            anim.SetBool(Jet, false);
            playerSprite.flipX = false;
            if (!Mathf.Approximately(src.pitch, 1.0f))
            {
                src.pitch = 1.0f; // Reset pitch when stopping
            }

            if (src.isPlaying)
            {
                src.Stop();

            }
            
        }

        private IEnumerator LoopWaterSound()
        {
            while (UserInput.instance.controls.Movement.Hose.IsPressed())
            {
                yield return new WaitForSeconds(0.4f); // Wait for sound to finish
                if (!UserInput.instance.controls.Movement.Hose.IsPressed())
                {
                    yield break;
                }
                src.pitch += 0.7f; // Increase pitch
                // src.Play(); // Replay sound
            }
        }

        private void Aim()
        {
            Vector2 direction = UserInput.instance.controls.Movement.Aim.ReadValue<Vector2>();
            if (direction == Vector2.zero)
            {
                // spawnPointSpriteRenderer.enabled = true;
                // set the direction to the right if facing right and left if facing left
                direction = spongeMovement.IsFacingRight ? Vector2.right : Vector2.left;
                
                // return;
            }
            
            
            if (direction.magnitude < 0.1) return;

            // spawnPointSpriteRenderer.enabled = true;

            // Normalize the aiming direction
            direction.Normalize();
            
            // Snap the direction to the closest 5 degree increment
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 10) * 10;
            direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Get the bounds of the player's collider
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider == null)
            {
                Debug.LogWarning("Player Collider2D is missing!");
                return;
            }

            // Calculate the edge of the collider in the aiming direction
            Bounds bounds = playerCollider.bounds;
            Vector3 colliderCenter = bounds.center;
            Vector3 colliderExtents = bounds.extents;
            

            // Determine the edge point based on the direction
            float xEdge = direction.x > 0 ? colliderExtents.x : -colliderExtents.x;
            float yEdge = direction.y > 0 ? colliderExtents.y : -colliderExtents.y;
            Vector3 edgePoint = colliderCenter + new Vector3(xEdge, yEdge, 0);

            // Adjust the edge point to align with the direction
            Ray2D ray = new Ray2D(colliderCenter, direction);
            Vector3 adjustedPoint = ray.GetPoint(Mathf.Max(Mathf.Abs(xEdge), Mathf.Abs(yEdge)));

            // Apply an offset to the spawn point
            Vector3 offset = direction * spawnDistance; // Customize spawnDistance for how far out the spawn point should be
            // calculate the size of the sprite of the player
            
            waterSpawnPoint.position = (adjustedPoint + offset) + (playerSprite.bounds.size.y / 2 * Vector3.up);

            // Rotate the spawn point to face the aiming direction
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            
            // standardize the angle to be between 0 and 360
            if (angle < 0)
            {
                angle += 360;
            }
            aimAngle = angle;
            
            if (angle > 90 && angle < 270)
            {
                fishSprite.flipY = true;
            }
            else
            {
                fishSprite.flipY = false;
            }
            
            
            waterSpawnPoint.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // Update the current shooting direction
            currentShotDirection = direction;
        }

        private void Shoot(bool isJet = false)
        {
            // Instantiate the water shot
            if (!isJet)
            {
                // Instantiate(waterPrefab, waterSpawnPoint.position, waterSpawnPoint.rotation);
                // StartCoroutine(ShootCooldown());
                // StartCoroutine(DecreaseSize());
                // splashEffect.Play();
                // StopCoroutine(StopTrail());
                // waterTrail.Play();
                // StartCoroutine(StopTrail());
                // src.PlayOneShot(splashSound);
            }
            else
            {
                if (spongeMovement.isFacingRight && aimAngle > 90 && aimAngle < 270)
                {
                    playerSprite.flipX = true;
                }
                else if (!spongeMovement.isFacingRight && (aimAngle < 90 || aimAngle > 270))
                {
                    playerSprite.flipX = true;
                }
                else
                {
                    playerSprite.flipX = false;
                }
                DecreaseSizeJet();
            }
            
            // Apply recoil to the player
            if (player.GetGroundType() == GroundTypes.StickySurface) return;
            Vector2 force = ApplyRecoil(isJet);
            // print($"FORCE IS :{force.normalized}");
            rb.AddForce(force, ForceMode2D.Impulse);

            CheckForAppliedForceFromAbove(force.normalized);
            

            // Determine the direction of the force and play the appropriate sound
            Vector2 upward = Vector2.up;
            float angle = Vector2.Angle(force, upward);
            
            
        }

        
        private void CheckForAppliedForceFromAbove(Vector2 force)
        {
            upwardsShotThreshold = -0.85f;
            if (force.y <= upwardsShotThreshold)
            {
                player.ApplyForceFromAbove(force);
            }
        }

        private IEnumerator StopTrail()
        {
            yield return new WaitForSeconds(0.5f);
            print("STOP TRAIL");
            // waterTrail.Stop();
        }

        private Vector2 ApplyRecoil(bool isJet = false)
        {
            if (rb.linearVelocity.y <= 0)
            {
                if (aimAngle is < 70 or > 110)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                }
            } 
            Vector2 recoilDirection = -currentShotDirection;
            
            // check if shooting upwards with a threshold
            // if (aimAngle is > 70 and < 110)
            // {
            //     recoilDirection.y *= 10f;
            // }
            
            // Adjust for diagonal aiming
            if (currentShotDirection.y < downShotYValueThreshold) // Diagonal shot
            {
                recoilDirection.y *= diagonalVerticalBias;   // Reduce vertical movement
            }
            recoilDirection.x *= horizontalForce;
            recoilDirection.y *= verticalForce;
            if (isJet)
            {
                recoilDirection.x *= 0.05f;
                recoilDirection.y *= 0.05f;
            }
            // Apply horizontal and vertical force components
            if (Mathf.Abs(currentShotDirection.y) < sideShootThreshold)
            {
                // print("1111 SIDE SHOOT - REDUCE X FORCE");
                recoilDirection.x *= reduceXRecoil;
            }
           

            // Check the player's velocity and increase force if it's low
            if (rb.linearVelocity.magnitude < lowVelocityThreshold)
            {
                // print("ADDED LOW VEL FORCE!!");
                recoilDirection.x *= forceIncreaseMultiplierX;
                recoilDirection.y *= forceIncreaseMultiplierY;
            }

            return recoilDirection;
        }
        
        private void DecreaseSizeJet()
        {
            // Change the local scale of the player to be between the min and max size based on the current water level
            Vector3 targetScale = Vector3.Lerp(player.FishMinSize, player.FishMaxSize, player.CurrentWater / player.MaxWater);
            fishSprite.transform.localScale = targetScale;
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
            // transform.localScale = player.MaxSize * (player.CurrentWater / player.MaxWater);;
        }
        
        
        private IEnumerator DecreaseSize()
        {
            Vector3 targetScale = transform.localScale - sizeDecreasePerShot;
            
            // Ensure the target size does not go below the minimum
            
            Vector3 initialScale = transform.localScale;
            float elapsedTime = 0;

            while (elapsedTime < shootCooldown)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / shootCooldown);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
        }

        public void FlipSpawnPoint()
        {
            defaultSpawnPointPosition.x *= -1;
        }

        private IEnumerator ShootCooldown()
        {
            onCooldown = true;
            yield return new WaitForSeconds(shootCooldown);
            onCooldown = false;
        }
    }
}
