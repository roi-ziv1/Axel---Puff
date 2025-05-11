// using System;
// using System.Collections;
// using Deprecated.Managers;
// using UnityEngine;
// using DG.Tweening;
// using DoubleTrouble.Interfaces;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;
// using CoreManager = SpongeScene.Managers.CoreManager;
//
// namespace DoubleTrouble.Managers
// {
//     public class Player : MonoBehaviour, ITakeDamage
//     {
//         [Header("ID")] [SerializeField] private int playerID;
//
//         [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
//         private bool isFacingRight = true;
//
//         [Header("Jump")] [SerializeField] private float jumpForce = 10f; // Force for manual jump
//         private bool toJump = false;
//
//         [Header("Shoot")] [SerializeField] private GameObject bulletPrefab;
//         [SerializeField] private float bulletSpeed = 10f;
//         [SerializeField] private GameObject aimDot;
//         [SerializeField] private GameObject bulletSpawn;
//         [SerializeField] private int maxBullets = 35;
//         [SerializeField] private float shootCooldown = 0.3f;
//
//         [SerializeField] private int currentBullets;
//         private readonly float dotDistance = 1f;
//         private float shootTimer = 0f;
//
//         [Header("Stick Together")] [SerializeField]
//         private Player otherPlayer;
//
//         private static float stickDistance = 20f;
//         public bool sticking = false;
//
//
//         private Tween squish;
//         private static bool[] mergePressed = { false, false };
//         private Rigidbody2D rb;
//         private float moveInput;
//         private bool isGrounded;
//
//         private float[] mergeInputCounter = { 0f, 0f };
//         private const float mergeInputFrameLimit = 0.3f;
//         
//         private bool squishedAlready = false;
//
//
//         private void Awake()
//         {
//             CoreManager.Instance.players.Add(this);
//         }
//
//         void Start()
//         {
//             rb = GetComponent<Rigidbody2D>();
//             currentBullets = 18;
//             ChangeSize();
//         }
//
//         void Update()
//         {
//             if (sticking) return;
//             Move();
//             Jump();
//             Aim();
//             Shoot();
//             Merge();
//             ChangeSize();
//             if (transform.position.y < -10)
//             {
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//             }
//         }
//
//         void Move()
//         {
//             moveInput = UserInput2P.instance.movementInput[playerID].x;
//             isFacingRight = moveInput switch
//             {
//                 > 0 => true,
//                 < 0 => false,
//                 _ => isFacingRight
//             };
//
//             rb.linearVelocityX = moveInput * moveSpeed;
//         }
//
//         void Jump()
//         {
//             // get the input from the right gamepad
//             if (!UserInput2P.instance.controls[playerID].Movement.Jump.WasPressedThisFrame()) return;
//             isGrounded = CheckGrounded();
//             if (isGrounded)
//             {
//                 toJump = true;
//             }
//         }
//
//         bool CheckGrounded()
//         {
//             LayerMask groundLayer = LayerMask.GetMask("Ground");
//             RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
//             return hit.collider != null;
//         }
//
//         void Shoot()
//         {
//             if (!UserInput2P.instance.controls[playerID].Movement.Shoot.WasPressedThisFrame()) return;
//             // shoot bullet where player is aiming
//             if (shootTimer > 0) return;
//             if (currentBullets <= 0) return;
//             Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
//             StartCoroutine(ShotCooldown());
//             currentBullets--;
//             // ChangeSize();
//         }
//
//         // shot cooldown as a coroutine
//         IEnumerator ShotCooldown()
//         {
//             shootTimer = shootCooldown;
//             yield return new WaitForSeconds(shootCooldown);
//             shootTimer = 0;
//         }
//
//
//         void Aim()
//         {
//             Vector2 aimDirection = UserInput2P.instance.aimInput[playerID];
//             if (aimDirection == Vector2.zero)
//             {
//                 aimDirection = isFacingRight ? Vector2.right : Vector2.left;
//             }
//
//             float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
//             // rotate the dot around the player to face the direction of the aim
//             aimDot.transform.position = transform.position + ((Vector3)aimDirection.normalized * dotDistance);
//             bulletSpawn.transform.rotation = Quaternion.Euler(0, 0, angle);
//         }
//
//         void Merge()
//         {
//             if (otherPlayer.sticking)
//                 return;
//             Vector2 targetPosition = Vector2.zero;
//             if (UserInput2P.instance.controls[playerID].Movement.MergeLeft.IsPressed())
//             {
//                 mergeInputCounter[playerID] = mergeInputFrameLimit;
//             }
//             else if (mergeInputCounter[playerID] > 0)
//             {
//                 mergeInputCounter[playerID] -= Time.deltaTime;
//             }
//
//             mergePressed[playerID] = mergeInputCounter[playerID] > 0;
//
//
//             if (!mergePressed[0] && !mergePressed[1]) return;
//             switch (mergePressed[playerID])
//             {
//                 case true when mergePressed[1 - playerID]:
//                     targetPosition = (transform.position + otherPlayer.gameObject.transform.position) / 2;
//                     break;
//                 case true when !mergePressed[1 - playerID]:
//                     targetPosition = otherPlayer.gameObject.transform.position;
//                     break;
//                 case false when mergePressed[1 - playerID]:
//                     return;
//                 default:
//                     return;
//             }
//
//             // check the number of objects between the two players
//             LayerMask lm = ~(1 << LayerMask.NameToLayer("Ground"));
//             int objectsBetween = Physics2D.RaycastAll(transform.position, otherPlayer.gameObject.transform.position, lm)
//                 .Length;
//             if (objectsBetween > 1) return;
//
//             // check if the players are close enough and make them move together 
//             if (Vector2.Distance(transform.position, otherPlayer.gameObject.transform.position) < stickDistance)
//             {
//                 rb.linearVelocity = Vector2.zero;
//                 rb.bodyType = RigidbodyType2D.Kinematic;
//                 rb.simulated = false;
//                 rb.bodyType = RigidbodyType2D.Kinematic;
//                 sticking = true;
//                 // CollisionChecker.OnBothPlayersColliding += Squish;
//                 // move to the middle between the two players
//                 squish = rb.transform.DOMove(targetPosition, 1f);
//                 squish.OnComplete(() =>
//                 {
//                     rb.bodyType = RigidbodyType2D.Dynamic;
//                     rb.simulated = true;
//                     rb.bodyType = RigidbodyType2D.Dynamic;
//                     sticking = false;
//                     // CollisionChecker.OnBothPlayersColliding -= Squish;
//                 });
//
//                 squish.onKill += () =>
//                 {
//                     rb.bodyType = RigidbodyType2D.Dynamic;
//                     rb.simulated = true;
//                     rb.bodyType = RigidbodyType2D.Dynamic;
//                     sticking = false;
//                 };
//                 
//                 // check for collision with other objects while tweening
//                 squish.OnUpdate(() =>
//                 {
//                     // check if the players are colliding with another object. I it's Squish move the object with the player and destroy it when finishing the tween
//                     Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, lm);
//                     foreach (var collider in colliders)
//                     {
//                         // if (Vector2.Distance(transform.position, otherPlayer.gameObject.transform.position) < 0.3f)
//                         // {
//                         //     squish.Kill();
//                         // }
//
//                         if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
//                         {
//                             // move the squish object with the player but keep the distance between the objects
//                             collider.transform.position =
//                                 new Vector2(collider.transform.position.x + (transform.position.x - rb.position.x),
//                                     collider.transform.position.y + (transform.position.y - rb.position.y));
//                             
//                             // destroy the squish object when the tween is finished
//                             if (squishedAlready) continue;
//                             collider.gameObject.GetComponent<Enemy>().IsAlive = false;
//                             AddBullets(10);
//                             otherPlayer.AddBullets(10);
//                             squish.onKill += () =>
//                             {
//                                 collider.gameObject.SetActive(false);
//                                 ChangeSize();
//                                 squishedAlready = false;
//                             };
//                             squishedAlready = true;
//
//                         }
//                         else
//                         {
//                             squish.Kill();
//                         }
//                     }
//                 });
//             }
//         }
//
//         public void ResumeTween()
//         {
//             squish.Play();
//         }
//
//         private void FixedUpdate()
//         {
//             if (toJump)
//             {
//                 rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//                 toJump = false;
//             }
//         }
//
//         private void ChangeSize()
//         {
//             // change the size of the player based on the number of bullets according to the formula: f(x)=0.2+351−0.2​⋅x
//             float scale = 0.2f + ((1f - 0.2f) * currentBullets / maxBullets);
//             transform.localScale = new Vector3(scale, scale, 1);
//         }
//
//         public void TakeDamage(IDealDamage damager)
//         {
//             if (currentBullets < damager.GetDamage())
//             {
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//                 return;
//             }
//
//             currentBullets -= damager.GetDamage();
//             ChangeSize();
//         }
//
//         private void AddBullets(int bullets)
//         {
//             currentBullets += bullets;
//             if (currentBullets > maxBullets)
//             {
//                 currentBullets = maxBullets;
//             }
//
//             ChangeSize();
//         }
//     }
// }