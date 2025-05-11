// using System.Collections;
// using DoubleTrouble.Interfaces;
// using UnityEngine;
//
// public abstract class Turbine : MonoBehaviour
// {
//     public float rotationSpeedIncrement = 100f; // Speed added with each hit
//     public float slowdownRate = 20f; // Rate of slowdown
//     public float spinDuration;
//
//     private float currentRotationSpeed = 0f; // Current rotation speed
//     private float timeRemaining = 0f; // Time left for spinning
//     private bool isSpinning = false; // Whether the turbine is spinning
//
//     protected Coroutine LinkedObjectRotationCoroutine;
//     public GameObject linkedObject; // The object to hide (e.g., the door)
//
//     public abstract IEnumerator AffectLinkedObject();
//     void Update()
//     {
//         if (isSpinning)
//         {
//             // Rotate the turbine
//             transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);
//
//             // Reduce the speed according to the slowdown rate
//             currentRotationSpeed = Mathf.Max(0, currentRotationSpeed - slowdownRate * Time.deltaTime);
//
//             // Decrease the remaining time
//             timeRemaining -= Time.deltaTime;
//
//             // Stop spinning if time runs out and speed is 0
//             if (timeRemaining <= 0 && currentRotationSpeed <= 0)
//             {
//                 isSpinning = false;
//             }
//         }
//     }
//
//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         // Check if the object hitting is water (Fluid layer)
//         if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
//         {
//             StartSpinning();
//         }
//     }
//
//     private void StartSpinning()
//     {
//         // If the turbine is already spinning, increase speed
//         if (isSpinning)
//         {
//            return;
//         }
//         else
//         {
//             // Start spinning for the first time
//             currentRotationSpeed = rotationSpeedIncrement;
//             isSpinning = true;
//
//             // Rotate the linked object slightly
//             if (linkedObject != null && LinkedObjectRotationCoroutine == null)
//             {
//                 LinkedObjectRotationCoroutine = StartCoroutine(AffectLinkedObject());
//             }
//         }
//
//         // Reset the timer to 5 seconds
//         timeRemaining = spinDuration;
//     }
//
//   
// }


using System;
using System.Collections;
using System.Collections.Generic;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene.Obstacles.Turbines
{
    public abstract class Turbine : MonoBehaviour
    {
        public float rotationPerParticle;// Degrees of rotation per particle hit
        public float slowdownRate = 20f; // Rate of slowdown
        public float spinDuration; // Duration of spinning

        private float currentRotationSpeed = 0f; // Current rotation speed
        private float timeRemaining = 0f; // Time left for spinning
        private bool isSpinning = false; // Whether the turbine is spinning
        
        protected Coroutine LinkedObjectRotationCoroutine;
        public GameObject linkedObject; // The object to hide (e.g., the door)
        [SerializeField] private AudioClip rotationSound;
        [SerializeField] private AudioSource src;
        [SerializeField] protected List<Sprite> sprites;
        [SerializeField] protected SpriteRenderer _renderer;
        protected SpriteRenderer linkedObjectRenderer;
        [SerializeField] protected Sprite likedObjectActivationSprite;
        [SerializeField] protected Sprite linkedObjectDefaultSprite;
        [SerializeField] protected bool resetSpin;
        public abstract IEnumerator AffectLinkedObject();

        private Quaternion startingRotation;
        private Quaternion linkedObjectStartingRotation;
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
            _renderer.sprite = sprites[0];
            transform.rotation = startingRotation;
            linkedObject.transform.rotation = linkedObjectStartingRotation;
            linkedObjectRenderer.sprite = linkedObjectDefaultSprite;

        }

        private void Start()
        {
            startingRotation = transform.rotation;
            linkedObjectStartingRotation = linkedObject.transform.rotation;
            linkedObjectRenderer = linkedObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (isSpinning)
            {
                // Rotate the turbine
                transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

                // Reduce the speed according to the slowdown rate
                // currentRotationSpeed = Mathf.Max(0, currentRotationSpeed - slowdownRate * Time.deltaTime);

                // Decrease the remaining time
                timeRemaining -= Time.deltaTime;
                print(timeRemaining +" time remaining");
                print(currentRotationSpeed+ " rotation speed ");
                // Stop spinning if time runs out and speed is 0
                if (timeRemaining < 0)
                {
                    isSpinning = false;
                    src.Stop();
                }
               
            }
            else
            {
                src.Stop();
            }
            
            
        }

        private void OnParticleCollision(GameObject other)
        {
            // Check if the particle is from water (or a specific layer)
            
            
            print(transform.rotation.z);
            if (transform.rotation.z < 0.69)
            {
                RotateByParticle();

            }
            else
            {
                if (resetSpin)
                {
                    linkedObject.transform.rotation = new Quaternion(1,0, 0, 0);

                }
                src.Stop();
                
            }
            
        }

        private void RotateByParticle()
        {
            // Rotate the turbine slightly
            transform.Rotate(0, 0, rotationPerParticle);
            _renderer.sprite = transform.rotation.z switch
            {
                
                < 0.15f => sprites[1],
                < 0.28f => sprites[2],
                < 0.42f => sprites[3],
                < 0.56f => sprites[4],
                < 7f => sprites[5],
                _ => sprites[5]
            };
            

            // Rotate the linked object slightly
            if (linkedObject != null)
            {
                linkedObject.transform.Rotate(0, 0, rotationPerParticle);
                linkedObjectRenderer.sprite = likedObjectActivationSprite;
            }

            // Extend spin duration if already spinning
            if (isSpinning)
            {
                timeRemaining += spinDuration;
            }
            else
            {
                // Start spinning
                timeRemaining = spinDuration;
                isSpinning = true;
                print("START SPINNIGJ");
                src.Play();
            }
        }
    }
}
