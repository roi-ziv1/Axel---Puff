using System.Collections;
using SpongeScene.Managers;
using SpongeScene.Obstacles.Turbines;
using UnityEngine;

namespace SpongeScene.WaterTriggers
{
    public class RotationTurbine : Turbine
    {
        [SerializeField] private int rotationDir;
        
        public override IEnumerator AffectLinkedObject()
        {
            yield break;
            // CoreManager.Instance.SoundManager.PlaySound(rotationSound);
            float rotationAngle = 90f; // Amount to rotate in degrees

            Quaternion initialRotation = linkedObject.transform.rotation;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, rotationAngle * rotationDir);

            float elapsedTime = 0f;

            while (elapsedTime < spinDuration)
            {
                linkedObject.transform.rotation =
                    Quaternion.Lerp(initialRotation, targetRotation, elapsedTime / spinDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            linkedObject.transform.rotation = targetRotation;
            LinkedObjectRotationCoroutine = null;
        }
    }
}