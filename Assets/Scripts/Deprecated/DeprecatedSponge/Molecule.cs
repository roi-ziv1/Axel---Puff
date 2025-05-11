using System;
using System.Collections;
using UnityEngine;

namespace SpongeScene
{
    public class Molecule : MonoBehaviour, IAbsorbable
    {
        public bool IsAbsorbed => isAbsorbed;
        private Coroutine cancelIsAbsorbedRoutine;
        private bool isAbsorbed = false;
        private Rigidbody2D rb;

        // Define the layers
        private const string NonPhysicsFluidLayer = "NonPhysicalFluid"; // The layer to set when absorbed
        private const string FluidLayer = "Fluid"; // The layer to set when released

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void OnAbsorb()
        {
            isAbsorbed = true;
            // Change the layer to NonPhysicsFluidLayer when absorbed
            gameObject.layer = LayerMask.NameToLayer(NonPhysicsFluidLayer);
            gameObject.SetActive(false);
        }

        public void Absorb()
        {
            gameObject.SetActive(false); // Hide the molecule when absorbed
        }

        public void OnRelease()
        {
            gameObject.SetActive(true); // Reappear the molecule when released
        }

        public void Release()
        {
            this.StopAndStartCoroutine(ref cancelIsAbsorbedRoutine, AllowToBeAbsorbedAfterDelay());
            // Change the layer back to FluidLayer when released
            gameObject.layer = LayerMask.NameToLayer(FluidLayer);
        }

        private IEnumerator AllowToBeAbsorbedAfterDelay()
        {
            // Wait until the rigidbody has minimal force (indicating it has stopped moving)
            
            yield return new WaitUntil(() => rb.bodyType != RigidbodyType2D.Kinematic );
            yield return new WaitForSeconds(0.5f);
            isAbsorbed = false;
        }
    }
}