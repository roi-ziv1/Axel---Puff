using System;
using System.Collections;
using DoubleTrouble.Managers;
using UnityEngine;
using DG.Tweening;
using SpongeScene;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayersMovement : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int playerID;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private bool isFacingRight = true;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f; // Force for manual jump
    private bool toJump = false;
    
    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private GameObject aimDot;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private int maxBullets = 35;
    [SerializeField] private float shootCooldown = 0.3f;
    
    private int currentBullets;
    private readonly float dotDistance = 1f;
    private float shootTimer = 0f;
    
    [Header("Stick Together")]
    [SerializeField] private GameObject otherPlayer;
    private static float stickDistance = 20f;
    private bool sticking = false;
    
    
    
    private Tween squish;
    private static bool[] mergePressed = {false, false};
    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentBullets = maxBullets;
    }

    void Update()
    {
        if (sticking) return;
        Move();
        Jump();
        Aim();
        Shoot();
        Merge();
    }

    void Move()
    {
      
        moveInput = UserInput2P.instance.movementInput[playerID].x;
        isFacingRight = moveInput switch
        {
            > 0 => true,
            < 0 => false,
            _ => isFacingRight
        };

        rb.linearVelocityX = moveInput * moveSpeed;
    }
    
    void Jump()
    {
        // get the input from the right gamepad

        if (!UserInput2P.instance.jumpInput[playerID]) return;
        isGrounded = CheckGrounded();
        if (isGrounded)
        {
            toJump = true;
        }
    }
    
    bool CheckGrounded()
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground"); 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }

    void Shoot()
    {
        if (!UserInput2P.instance.shootInput[playerID]) return;
        // shoot bullet where player is aiming
        if(shootTimer > 0) return;
        if (currentBullets <= 0) return;
        Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        StartCoroutine(ShotCooldown());
        currentBullets--;
        changeSize();
    }
    
    // shot cooldown as a coroutine
    IEnumerator ShotCooldown()
    {
        shootTimer = shootCooldown;
        yield return new WaitForSeconds(shootCooldown);
        shootTimer = 0;
    }
    
    
    void Aim()
    {
        Vector2 aimDirection = UserInput2P.instance.aimInput[playerID];
        if (aimDirection == Vector2.zero)
        {
            aimDirection = isFacingRight ? Vector2.right : Vector2.left;
        }
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        // rotate the dot around the player to face the direction of the aim
        aimDot.transform.position = transform.position + ((Vector3)aimDirection.normalized * dotDistance);
        bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Merge()
    {
        Vector2 targetPosition = Vector2.zero;
        if (UserInput2P.instance.mergeInput1[playerID])
        {
            mergePressed[playerID] = true;
        }

        else
        {
            mergePressed[playerID] = false;
        }
        
        Debug.Log(mergePressed[0] + " " + mergePressed[1]);
        
        if (!mergePressed[0] && !mergePressed[1]) return;
        switch (mergePressed[playerID])
        {
            case true when mergePressed[1 - playerID]:
                targetPosition = (transform.position + otherPlayer.transform.position) / 2;
                break;
            case true when !mergePressed[1 - playerID]:
                targetPosition = otherPlayer.transform.position;
                break;
            case false when mergePressed[1 - playerID]:
                return;
            default:
                return;
        }
        
        // check the number of objects between the two players
        LayerMask lm = ~(1 << LayerMask.NameToLayer("Ground"));
        int objectsBetween = Physics2D.RaycastAll(transform.position, otherPlayer.transform.position, lm).Length;
        print(objectsBetween);
        if (objectsBetween > 1) return;

        // check if the players are close enough and make them move together 
        if (Vector2.Distance(transform.position, otherPlayer.transform.position) < stickDistance)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            sticking = true;
            CollisionChecker.OnBothPlayersColliding += Squish;
            // move to the middle between the two players
            squish = rb.transform.DOMove(targetPosition , 1f).OnComplete(() =>
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.simulated = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                sticking = false;
                CollisionChecker.OnBothPlayersColliding -= Squish;
            });
            //layer mask to ignore collisions with the ground
            
            // check for collision with other objects while tweening
            squish.OnUpdate(() =>
            {
                // check if the players are colliding with another object. I it's Squish move the object with the player and destroy it when finishing the tween
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, lm);
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Squish"))
                    {
                        // move the squish object with the player but keep the distance between the objects
                        collider.transform.position = new Vector2(collider.transform.position.x + (transform.position.x - rb.position.x), collider.transform.position.y + (transform.position.y - rb.position.y));
                        // destroy the squish object when the tween is finished
                        squish.onKill += () =>
                        {
                            Destroy(collider.gameObject);
                            currentBullets += 10;
                            changeSize();
                        };
                    }
                    else
                    {
                        squish.Kill();
                    }
                }
                
            });
            squish.onKill += () =>
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.simulated = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.constraints = RigidbodyConstraints2D.None;
                sticking = false;
            };
        }
    }
    
    public void ResumeTween()
    {
        squish.Play();
    }
    
    private void Squish(GameObject toSquish)
    {
        if (sticking)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.None;
            sticking = false;
            Destroy(toSquish);
        }
    }

    private void FixedUpdate()
    {
        if (toJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            toJump = false;
        }
    }

    private void changeSize()
    {
        // The size change to currentBullet/maxBullets
        transform.localScale = new Vector3(currentBullets / maxBullets, currentBullets / maxBullets, 1);
    }
}