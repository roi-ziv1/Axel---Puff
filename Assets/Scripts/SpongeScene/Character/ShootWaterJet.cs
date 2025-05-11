using System.Collections;
using Character;
using UnityEngine;

namespace SpongeScene.Character
{
    public class ShootWaterJet : MonoBehaviour
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
        // [SerializeField] private AudioSource src;
        // [SerializeField] private AudioClip upwardsSound;
        // [SerializeField] private AudioClip[] downwardSounds;
        [SerializeField] private ParticleSystem waterHose;
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
        private double upwardsShotThreshold;

        
        void Start()
        {
            spawnPointSpriteRenderer = waterSpawnPoint.GetComponent<SpriteRenderer>();
            defaultSpawnPointPosition = waterSpawnPoint.localPosition;
            absorbWater = GetComponent<AbsorbWater>();
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerManager>();
            sizeDecreasePerShot = (player.MaxSize - player.MinSize) / player.MaxWater;
            spongeMovement = GetComponent<SpongeMovement>();
            waterTrail.Stop();
            waterHose.Stop();
        }

        void Update()
        {
            Aim();
        }

        private void FixedUpdate()
        {
            if (UserInput.instance.controls.Movement.Hose.IsPressed() && !onCooldown)
            {
                Shoot();
            }
            else if(UserInput.instance.controls.Movement.Hose.IsPressed() == false)
            {
                waterHose.Stop();
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
            waterSpawnPoint.position = (adjustedPoint + offset);

            // Rotate the spawn point to face the aiming direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            waterSpawnPoint.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // Update the current shooting direction
            currentShotDirection = direction;
        }

        private void Shoot()
        {
            // Instantiate the water shot
            // Instantiate(waterPrefab, waterSpawnPoint.position, waterSpawnPoint.rotation);
            waterHose.Play();
            // StartCoroutine(ShootCooldown());
            // StartCoroutine(DecreaseSize());
            splashEffect.Play();
            StopCoroutine(StopTrail());
            waterTrail.Play();
            StartCoroutine(StopTrail());

            // Apply recoil to the player
            if (player.GetGroundType() == GroundTypes.StickySurface) return;
            Vector2 force = ApplyRecoil();
            // print($"FORCE IS :{force.normalized}");
            rb.AddForce(force, ForceMode2D.Force);

            CheckForAppliedForceFromAbove(force.normalized);
            

            // Determine the direction of the force and play the appropriate sound
            Vector2 upward = Vector2.up;
            float angle = Vector2.Angle(force, upward);
            
            // src.PlayOneShot(upwardsSound);
            
          
        }

        private void CheckForAppliedForceFromAbove(Vector2 force)
        {
            upwardsShotThreshold = -0.85;
            if (force.y <= upwardsShotThreshold)
            {
                player.ApplyForceFromAbove(force);
            }
        }

        private IEnumerator StopTrail()
        {
            yield return new WaitForSeconds(0.5f);
            print("STOP TRAIL");
            waterTrail.Stop();
        }

        private Vector2 ApplyRecoil()
        {
            if (rb.linearVelocity.y <= 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            } 
            Vector2 recoilDirection = -currentShotDirection;

            // Adjust for diagonal aiming
            if (currentShotDirection.y < downShotYValueThreshold) // Diagonal shot
            {
                recoilDirection.y *= diagonalVerticalBias;   // Reduce vertical movement
            }
            recoilDirection.x *= horizontalForce;
            recoilDirection.y *= verticalForce;
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
