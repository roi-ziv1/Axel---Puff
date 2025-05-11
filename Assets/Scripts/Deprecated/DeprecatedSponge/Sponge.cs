using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace SpongeScene
{
    public class Sponge : MonoBehaviour, IInteractable
    {
        
        public float circleRadius = 1f; // Radius of the CircleCast
        public float absorbCooldown = 0.02f; // Cooldown between absorbs

        private List<Molecule> neededToAbsorbArray = new List<Molecule>(); // Molecules to absorb
        private float nextAbsorbTime; // Tracks cooldown time


        [SerializeField] private float absorbSpeed = 1f;
        [SerializeField] private float sizeIncreasePerMolecule = 0.3f;
        [SerializeField] private int absorbCapacity;
        [SerializeField] float forceMagnitude = 10f; // Adjust force magnitude as needed

        [SerializeField] private GameObject[] joints;

        private static int _defaultLayer;
        private static int _absorbLayer;
        private int fluidsLayer; // Layer for "Fluids"


        private Rigidbody2D _rb;
        private readonly List<Molecule> absorbedMolecules = new List<Molecule>();
        private float mouseHoldTime = 0f;
        private bool isHoldingSpace = false;
        private bool isInteracted = false;
        private bool releasing = false;
        private Coroutine releaseCoroutine;
        


        public Transform objectTransform => transform;

        private void Start()
        {
           
            _rb = GetComponent<Rigidbody2D>();
        }


        void Update()
        {
            HandleInput();

            if (!releasing)
            {
                FindMoleculesToAbsorb();
            }

            TryToAbsorbMolecule();
        }

        private void TryToAbsorbMolecule()
        {
            // Process absorption from the list
            if (releasing)
            {
                // using dotween to shake the sponge
                foreach (var mol in neededToAbsorbArray)
                {
                    mol.Release();
                }
                neededToAbsorbArray.Clear();
            }
            if (neededToAbsorbArray.Count > 0 && Time.time >= nextAbsorbTime && UserInput.instance.controls.Movement.Absorb.IsPressed())
            {
                AbsorbMolecule(neededToAbsorbArray[^1]); // Absorb the first molecule in the list
                neededToAbsorbArray.RemoveAt(neededToAbsorbArray.Count -
                                             1); // Remove the absorbed molecule from the list
                nextAbsorbTime = Time.time + absorbCooldown; // Set the next absorb time
            }
        }

        // private void OnCollisionStay2D(Collision2D other)
        // {
        //     print("COLLLLL");
        //     if (gameObject.layer == _absorbLayer)
        //     {
        //         print("LETS GO SUCK!");
        //         Molecule molecule = other.gameObject.GetComponent<Molecule>();
        //         if (absorbCapacity > absorbedMolecules.Count && molecule != null && molecule.IsAbsorbed == false)
        //         {
        //             AbsorbMolecule(molecule);
        //         }
        //     }
        // }

        private void HandleInput()
        {
            if (UserInput.instance.controls.Movement.Shoot.WasPressedThisFrame())
            {
                isHoldingSpace = true;
                mouseHoldTime = 0f; // Reset hold time when the spacebar is initially pressed
            }


            // Accumulate hold time while the spacebar is held down
            if (releaseCoroutine == null && isHoldingSpace && absorbedMolecules.Count > 0 && UserInput.instance.controls.Movement.Shoot.WasReleasedThisFrame())
            {                
                mouseHoldTime += Time.deltaTime;
                ReleaseAllWater();
                releasing = true;
            }

            if (UserInput.instance.controls.Movement.Shoot.WasReleasedThisFrame())
            {
                isHoldingSpace = false;
                mouseHoldTime = 0;
                releasing = false;
            }
        }


        public void OnInteract()
        {
            _rb.bodyType = RigidbodyType2D.Kinematic; // Make the Rigidbody kinematic to disable physics
            isInteracted = true;

            // Calculate random percentage (5-10%) of water to release
            int moleculesToRelease = Mathf.CeilToInt(absorbedMolecules.Count * Random.Range(0.05f, 0.1f));
            for (int i = 0; i < moleculesToRelease; i++)
            {
                if (absorbedMolecules.Count > 0)
                {
                    ReleaseLiquidOverTime();
                }
            }
        }

        public void OnDrop()
        {
            _rb.bodyType = RigidbodyType2D.Dynamic; // Re-enable physics on drop
            isInteracted = false;
        }


        private void ReleaseAllWater()
        {
            for (int i = 0; i < absorbedMolecules.Count; ++i)
            {
                StartCoroutine(ReleaseMolecule());
            }
        }

        private void AbsorbMolecule(Molecule molecule)
        {
            // Calculate how much the object needs to be moved up to avoid collision during scaling
            Vector3 scaleDelta = Vector3.one * sizeIncreasePerMolecule;
            Vector3 moveUp = Vector3.up * (scaleDelta.y);  // Move upwards based on the Y-axis scaling
            transform.position += moveUp; // Ensure the object moves upwards as it scales

        
            // Start moving the object while scaling
            StartCoroutine(UtilityFunctions.MoveObjectOverTime(molecule.gameObject, molecule.transform.position,
                molecule.transform.rotation, transform, molecule.transform.rotation, 1));

            // Absorb the molecule and update its scale
            molecule.Absorb();
            absorbedMolecules.Add(molecule);
    
            // Apply the size increase and the upward movement
            transform.localScale += scaleDelta;
            
        }
        
        

        private IEnumerator ReleaseLiquidOverTime()
        {
            while (isHoldingSpace && absorbedMolecules.Count > 0)
            {
                StartCoroutine(ReleaseMolecule());
                yield return new WaitForSeconds(0.06f);
            }

            releaseCoroutine = null;
        }

        private IEnumerator ReleaseMolecule()
        {
            // Get the last absorbed molecule and remove it from the list
            Molecule molecule = absorbedMolecules[absorbedMolecules.Count - 1];
            absorbedMolecules.RemoveAt(absorbedMolecules.Count - 1);

            // Set the molecule's position to the sponge's position
            molecule.transform.position = transform.position;

            // Apply a force to the molecule's Rigidbody2D
            Rigidbody2D moleculeRb = molecule.GetComponent<Rigidbody2D>();
            if (moleculeRb != null)
            {
                molecule.OnRelease();
                moleculeRb.linearVelocity = Vector2.zero; // Reset velocity before applying force

                // Determine the direction based on the character's facing direction
                float direction = transform.rotation.y > 0 ? 1 : -1;
                Vector2 force = new Vector2(direction * forceMagnitude, 0); // Horizontal force only
                moleculeRb.AddForce(force, ForceMode2D.Impulse);
            }

            // Add the WaterState component to manage the state of the released molecule
            WaterState waterState = molecule.gameObject.GetComponent<WaterState>();
            if (waterState == null)
            {
                waterState = molecule.gameObject.AddComponent<WaterState>();
            }
            waterState.SetFired(); // Mark this molecule as "fired"

            // Decrease the size of the releasing object
            transform.localScale -= Vector3.one * sizeIncreasePerMolecule;

            yield return new WaitForSeconds(0.2f); // Wait before releasing molecule completely
            molecule.Release();
        }



        // Helper to get a random point on the sponge's edge
        private Vector3 GetRandomPerimeterPoint()
        {
            // Get the BoxCollider2D component and check if it exists
            Collider2D boxCollider = GetComponent<Collider2D>();
            if (boxCollider == null)
            {
                Debug.LogWarning("No BoxCollider2D attached to the Sponge object.");
                return transform.position; // Fallback to the object's position if no collider is found
            }

            // Get the bounds of the collider
            Bounds bounds = boxCollider.bounds;

            // Perimeter lengths of each side
            float minX = bounds.min.x;
            float maxX = bounds.max.x;
            float minY = bounds.min.y;
            float maxY = bounds.max.y;
            float randX = Random.Range(minX, maxX);
            float randY = Random.Range(minY, maxY);
            print($"x : {randX} y : {randY}");
            return new Vector3(randX, randY, 0);
        }


// Helper to find the vicinity around the closest ground point
        private Vector3 GetGroundVicinityPoint()
        {
            // Get the BoxCollider2D component and check if it exists
            Collider2D boxCollider = GetComponent<Collider2D>();

            // Calculate the lowest point based on the collider's bounds
            float lowestY = boxCollider.bounds.min.y;
            float minX = boxCollider.bounds.min.x;
            float maxX = boxCollider.bounds.max.x;

            return new Vector3(Random.Range(minX, maxX), lowestY + +Random.Range(0, 0.1f), 0);
        }


// Coroutine to lerp the molecule to the ground vicinity
        private IEnumerator LerpToGround(Molecule molecule, Vector3 releasePoint, Vector3 groundVicinityPoint)
        {
            float distanceThreshold = 0.2f;
            float spongeMovementLimit = 0.3f;
            float duration = 1f;
            Vector3 initialSpongePosition = transform.position;

            molecule.transform.position = releasePoint;

            // Temporarily disable physics by setting isKinematic to true
            molecule.OnRelease();

            float completed = 0;
            while (Vector3.Distance(molecule.transform.position, groundVicinityPoint) > distanceThreshold)
            {
                // Check if the sponge moved beyond the allowed threshold
                if (Vector3.Distance(transform.position, initialSpongePosition) > spongeMovementLimit)
                {
                    molecule.Release();
                    yield break;
                }

                completed += Time.deltaTime;
                // Move molecule towards the ground vicinity point
                molecule.transform.position = Vector3.Lerp(releasePoint, groundVicinityPoint,
                    completed / duration);
                yield return null;
            }

            molecule.Release();
        }

        private void FindMoleculesToAbsorb()
        {
            // Get the Collider2D attached to this object
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                // Use the collider's bounds to define the overlap area
                Vector2 colliderSize = col.bounds.size;
        
                // Perform OverlapBox using the collider's size and position
                Collider2D hit = Physics2D.OverlapBox(transform.position, colliderSize, 0f); // 0f rotation
                
                if (hit != null)
                {
                    Molecule molecule = hit.GetComponent<Molecule>();
                    // Ensure the molecule is valid, not already absorbed, and not already in the list
                    if (molecule != null && !molecule.IsAbsorbed &&
                        neededToAbsorbArray.Count + absorbedMolecules.Count < absorbCapacity)
                    {
                        Molecule newMolecule = Instantiate(molecule, new (0,0,0), Quaternion.identity);

                        newMolecule.OnAbsorb(); // Custom absorb logic
                        neededToAbsorbArray.Add(newMolecule); // Add to the absorption list
                    }
                }
                else
                {
                    print("No molecules found within the collider area.");
                }
            }
            else
            {
                print("No Collider2D found on the object.");
            }
        }


        private void OnDrawGizmos()
        {
            // Draw the CircleCast radius in the Scene view
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, circleRadius);
        }
        
        public float GetCurrentSizeIncrease()
        {
            return sizeIncreasePerMolecule * absorbedMolecules.Count;
        }

        // public void DoubleJumpWaterRelease()
        // {
        //     if (absorbedMolecules.Count <= 3) return;
        //     // release 3 molecules
        //     for (int i = 0; i < 3; i++)
        //     {
        //         StartCoroutine(ReleaseMolecule());
        //     }
        // }

        public bool IsGrounded()
        {
            foreach (GameObject joint in joints)
            {

                if (Physics2D.OverlapCircle(joint.transform.position, joint.GetComponent<CircleCollider2D>().radius,
                        LayerMask.NameToLayer("Ground"))!= null)
                {
                    return true;
                }

            }

            return false;
        }
    }
}