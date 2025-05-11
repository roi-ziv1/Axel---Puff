// using System;
// using SpongeScene.Managers;
// using UnityEngine;
//
// namespace SpongeScene
// {
//     public class ObjectGrowth : MonoBehaviour
//     {
//         [Header("Growth Settings")]
//         public float growthRate = 0.6f; // Rate of gradual growth (units per second)
//         public float maxGrowthMultiplier = 3f; // Maximum size relative to the initial size
//         public float growthIncrement = 1f; // Amount to grow per collision
//
//         private Vector3 initialScale; // Stores the initial scale of the ball
//         private Vector3 currentTargetScale; // The next target scale for growth
//         private Vector3 initialPosition; // Stores the initial position of the ball
//         
//         [Header("Shrink Settings")]
//         public float shrinkRate = 0.6f; // Rate of gradual shrinking
//         public float shrinkDelay = 4.5f; // Time to wait before starting to shrink
//
//         private bool isGrowing = false;
//         private bool isShrinking = false;
//         private float lastGrowthTime; // Tracks the last time the object started growing
//
//         private void OnEnable()
//         {
//             CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetObject);
//         }
//
//         private void OnDisable()
//         {
//             CoreManager.Instance.EventsManager.RemoveListener(EventNames.Die, ResetObject);
//         }
//
//         private void ResetObject(object obj)
//         {
//             isGrowing = false;
//             isShrinking = false;
//             transform.localScale = initialScale;
//         }
//
//         void Start()
//         {
//             // Save the initial scale and position of the ball
//             initialScale = transform.localScale;
//             initialPosition = transform.position;
//
//             // Set the initial target scale
//             currentTargetScale = initialScale;
//
//             // Initialize lastGrowthTime
//             lastGrowthTime = Time.time;
//         }
//
//         void Update()
//         {
//             // Handle growth logic
//             if (isGrowing)
//             {
//                 // Gradually increase the scale of the object
//                 transform.localScale = Vector3.Lerp(transform.localScale, currentTargetScale, growthRate * Time.deltaTime);
//                 
//                 // Stop growing if the scale is close enough to the target
//                 if (Vector3.Distance(transform.localScale, currentTargetScale) < 0.01f)
//                 {
//                     transform.localScale = currentTargetScale; // Snap to the target scale
//                     isGrowing = false; // Stop growing
//                 }
//
//                 // Update last growth time
//                 lastGrowthTime = Time.time;
//                 isShrinking = false; // Stop shrinking if growing starts
//             }
//
//             // Handle passive shrinking logic
//             if (!isGrowing && Time.time - lastGrowthTime > shrinkDelay && transform.localScale.x > initialScale.x)
//             {
//                 isShrinking = true;
//             }
//
//             if (isShrinking)
//             {
//                 // Gradually decrease the scale of the object
//                 transform.localScale = Vector3.Lerp(transform.localScale, initialScale, shrinkRate * Time.deltaTime);
//
//                 // Stop shrinking if the scale is close enough to the initial size
//                 if (Vector3.Distance(transform.localScale, initialScale) < 0.01f)
//                 {
//                     transform.localScale = initialScale; // Snap to the initial scale
//                     isShrinking = false; // Stop shrinking
//                 }
//             }
//         }
//
//         private void OnCollisionEnter2D(Collision2D collision)
//         {
//             if (collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Water"))
//             {
//                 CoreManager.Instance.SoundManager.PlaySoundByName(SoundName.ObjectGrow);
//                 IncrementGrowth();
//             }
//         }
//
//         private void IncrementGrowth()
//         {
//             // Calculate the new target scale
//             Vector3 nextScale = transform.localScale + new Vector3(growthIncrement, 0, 0);
//
//             // Ensure the new scale does not exceed the maximum allowed size
//             currentTargetScale = new Vector3(
//                 Mathf.Min(nextScale.x, initialScale.x * maxGrowthMultiplier),
//                 Mathf.Min(nextScale.y, initialScale.y * maxGrowthMultiplier),
//                 Mathf.Min(nextScale.z, initialScale.z * maxGrowthMultiplier)
//             );
//             isGrowing = true;
//         }
//     }
// }
using System;
using SpongeScene.Managers;
using UnityEngine;

namespace SpongeScene
{
    public class ObjectGrowth : MonoBehaviour
    {
        [Header("Growth Settings")]
        public float growthRate = 0.6f; // Rate of gradual growth (units per second)
        public float maxGrowthMultiplier = 3f; // Maximum size relative to the initial size
        public float growthIncrement = 0.025f; // Amount to grow per particle

        private Vector3 initialScale; // Stores the initial scale of the object
        private Vector3 currentTargetScale; // The next target scale for growth

        [Header("Shrink Settings")]
        public float shrinkRate = 0.6f; // Rate of gradual shrinking
        public float shrinkDelay = 4.5f; // Time to wait before starting to shrink

        private bool isGrowing = false;
        private bool isShrinking = false;
        private float lastGrowthTime; // Tracks the last time the object started growing

        [SerializeField] private AudioSource src;
        

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
            isGrowing = false;
            isShrinking = false;
            transform.localScale = initialScale;
        }

        void Start()
        {
            // Save the initial scale of the object
            initialScale = transform.localScale;

            // Set the initial target scale
            currentTargetScale = initialScale;

            // Initialize lastGrowthTime
            lastGrowthTime = Time.time;
        }

        void Update()
        {
            // Handle growth logic
            if (isGrowing)
            {
                print("is growing11!");
                // Gradually increase the scale of the object
                transform.localScale = Vector3.Lerp(transform.localScale, currentTargetScale, growthRate * Time.deltaTime);

                // Stop growing if the scale is close enough to the target
                if (Vector3.Distance(transform.localScale, currentTargetScale) < 0.6f)
                {
                    print("target sclae reched ! exit");
                    transform.localScale = currentTargetScale; // Snap to the target scale
                    isGrowing = false; // Stop growing
                }

                // Update last growth time
                lastGrowthTime = Time.time;
                isShrinking = false; // Stop shrinking if growing starts
            }

            // Handle passive shrinking logic
            else if ( !isShrinking && !isGrowing && Time.time - lastGrowthTime > shrinkDelay && transform.localScale.x > initialScale.x)
            {
                print("shrinking!!");
                isShrinking = true;
                isGrowing = false;
                CoreManager.Instance.SoundManager.PlaySoundBySource(src,SoundName.ObjectDecrease);
            }

            else if(isShrinking)
            {
                // Gradually decrease the scale of the object
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, shrinkRate * Time.deltaTime);

                // Stop shrinking if the scale is close enough to the initial size
                if (Vector3.Distance(transform.localScale, initialScale) < 0.01f)
                {
                    transform.localScale = initialScale; // Snap to the initial scale
                    isShrinking = false; // Stop shrinking
                }
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            // Check if the colliding particle system is of interest

            if (!isGrowing)
            {
                CoreManager.Instance.SoundManager.PlaySoundBySource(src,SoundName.ObjectGrow);
            }
            IncrementGrowth();
           
        }

        private void IncrementGrowth()
        {
            // Calculate the new target scale
            Vector3 nextScale = transform.localScale + new Vector3(growthIncrement,0,0);

            // Ensure the new scale does not exceed the maximum allowed size
            currentTargetScale = new Vector3(
                Mathf.Min(nextScale.x, initialScale.x * maxGrowthMultiplier),
                Mathf.Min(nextScale.y, initialScale.y * maxGrowthMultiplier),
                Mathf.Min(nextScale.z, initialScale.z * maxGrowthMultiplier)
            );

            isGrowing = true;
        }
    }
}
