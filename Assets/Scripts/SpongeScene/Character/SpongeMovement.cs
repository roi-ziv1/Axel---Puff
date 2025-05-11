using System;
using SpongeScene;
using SpongeScene.Managers;
using UnityEngine;

namespace Character
{
    public class SpongeMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f; // Speed for smooth movement
        [SerializeField] private float maxSpeed = 25f; // Maximum speed for the player
        [SerializeField] private bool jumpKeyActivated = true;
        [Header("Jump")] [SerializeField] private float jumpForce = 10f; // Force for manual jump

        [SerializeField] private AudioSource src;
        [SerializeField] private AudioClip groundAudio;
        [SerializeField] private AudioClip waterAudio;
        [SerializeField] private AudioClip jumpSound;
        // [SerializeField] private float jumpCooldown = 1f; // Cooldown time for jumps
        
        private ShootWater shootWater;
        private Rigidbody2D rb;
        [SerializeField] private float moveInput;
        // private int groundLayer;
        // private int waterLayer;
        [SerializeField] private LayerMask jumpLayers;
        // private bool canDoubleJump = false;
        private bool canJump;
        [HideInInspector] public bool isFacingRight = true;
        private float jumpChargeTime = 0f;
        private bool onWater = false;
        
        [SerializeField] private Animator animator;

        public float curVelocity;
        private float jumpBufferTime = 0.1f;
        private float jumpBufferTimer = 0f;
        private float movingDuration;
        private PlayerManager player;
        private float spriteLowerBound;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Jumping = Animator.StringToHash("Jump");
        [SerializeField] private float coyoteTime = 0.2f;
        private float coyoteTimer = 0f;
        private bool grounded;
        

        void Start()
        {
            player = GetComponent<PlayerManager>();
            rb = GetComponent<Rigidbody2D>();
            // groundLayer = LayerMask.GetMask("Ground");
            // waterLayer = LayerMask.GetMask("Water");
            shootWater = GetComponent<ShootWater>();
            
            // Create a layer mask for both the ground and water layers
        }

        void Update()
        {
            // if (isAbsorbing)
            // {
            //     return;
            // }
            moveInput = UserInput.instance.movementInput.x;
            if (UserInput.instance.controls.Movement.Jump.WasPressedThisFrame())
            {
                jumpBufferTimer = jumpBufferTime;
            }
            HandleMovementSound();
            
            
            // Move(); // Add continuous movement
            // if (Input.GetKey(KeyCode.A))
            // {
            //     jumpKeyActivated = true;
            // }
            // if (Input.GetKey(KeyCode.S))
            // {
            //     jumpKeyActivated = false;
            // }
            
        }

        private void FixedUpdate()
        {
            grounded = IsGrounded();
            if (grounded)
            {
                coyoteTimer = coyoteTime;
                animator.SetBool(Jumping, false);
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
            }
            Move();
            if (jumpBufferTimer > 0)
            {
                jumpBufferTimer -= Time.fixedDeltaTime;
        
                if (IsGrounded() && jumpBufferTimer > 0)
                {
                    Jump();
                }
            }
            curVelocity = rb.linearVelocity.magnitude;
            // if(rb.linearVelocity.magnitude > maxSpeed)
            // {
            //     rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            // }
        }


        void Move()
        {
            
            
            // Ignore small inputs to avoid jitter
            if (Mathf.Abs(moveInput) < 0.03f || !player.CanMove)
            {
                movingDuration = 0;
                animator.SetBool(Moving, false);
                return;
            }
            movingDuration += Time.deltaTime;
            // Apply force for smooth horizontal movement
            float desiredSpeed = moveInput * moveSpeed;
            float currentSpeed = rb.linearVelocity.x;
            float speedDifference = desiredSpeed - currentSpeed;

            // Apply force proportional to the speed difference
            rb.AddForce(new Vector2(speedDifference, 0) * Time.fixedDeltaTime, ForceMode2D.Impulse);

            // Flip the character if moving in the opposite direction
            if (moveInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && isFacingRight)
            {
                Flip();
            }
            
            
            if (moveInput != 0 && grounded)
            {
                animator.SetBool(Moving, true);
            }
            else
            {
                animator.SetBool(Moving, false);
            }
        }



        void Flip()
        {
            // rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            shootWater.FlipSpawnPoint();
        }
       
        
        void Jump()
        {

            // if (!UserInput.instance.controls.Movement.Jump.WasPressedThisFrame()) return;
            canJump = grounded || coyoteTimer > 0;
            if ((rb.linearVelocity.y > 0.02 && !onWater) || !canJump)
            {
                return;
            }
    
            movingDuration = 0;
            CoreManager.Instance.SoundManager.PlaySound(jumpSound);
            // canJump = IsGrounded();
            //animator.SetBool("Grounded", canJump);
            // if (!canJump) return;
            if (onWater)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
            }
            animator.SetBool(Jumping, true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimer = 0;
            // animator.SetTrigger("Jump");
        }
        
        private bool IsGrounded()
        {
            // Check if the player is grounded
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
            // return hit.collider != null;
            // return player.IsOnGround();
            var hit = Physics2D.OverlapCircle(transform.position, 0.2f, jumpLayers);
            // draw the overlap circle
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.2f, Color.red, 0.1f);
            return hit != null;
            return Physics2D.Raycast(transform.position, Vector2.down, 0.15f, jumpLayers);
        }
        

        public bool Grounded => IsGrounded();

        // public void Absorb()
        // {
        //     isAbsorbing = true;
        //     // Over 1 second increase the scale of the sponge to 1.5x its original size
        //     transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), 1f);
        //     StartCoroutine(WaitSecond());
        // }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                onWater = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                onWater = false;
            }
        }
        
        void HandleMovementSound()
        {
            if (Mathf.Abs(moveInput) > 0.05f) // Movement is detected
            {
                switch (player.GetGroundType())
                {
                    case GroundTypes.Ground:
                    case GroundTypes.StickySurface:
                        // Check if the ground movement sound is already playing
                        if ((src.clip != groundAudio || !src.isPlaying) && movingDuration > 0.2f)
                        {
                            src.clip = groundAudio;
                            src.Play();
                        }
                        break;

                    case GroundTypes.Water:
                        // Check if the water movement sound is already playing
                        if ((src.clip != waterAudio || !src.isPlaying) && movingDuration > 0.2f)
                        {
                            src.clip = waterAudio;
                            src.Play();
                        }
                        break;
                    case GroundTypes.None:
                        if (src.clip == groundAudio || src.clip == waterAudio)
                        {
                            src.Stop();
                        }
                        break;

                }
            }
            else
            {
                // Stop the movement sound when there is no movement input
                if (src.isPlaying)
                {
                    src.Stop();
                }
            }
        }

        
        public bool IsFacingRight => isFacingRight;
    }
}