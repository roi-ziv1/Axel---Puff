using System.Collections;
using SpongeScene.Obstacles.Turbines;
using Unity.VisualScripting;
using UnityEngine;

namespace SpongeScene.Turbines
{
    public class MovementTurbine : Turbine
    {
        [SerializeField] private Transform newPosition;
        public override IEnumerator AffectLinkedObject()
        {
            LinkedObjectRotationCoroutine = StartCoroutine(UtilityFunctions.MoveObjectOverTime(linkedObject, linkedObject.transform.position, linkedObject.transform.rotation,
                newPosition, linkedObject.transform.rotation, spinDuration, () => LinkedObjectRotationCoroutine = null));
            yield return null;
        }
    }
}