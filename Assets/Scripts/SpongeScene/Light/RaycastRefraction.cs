using UnityEngine;

using UnityEngine;

public class RaycastRefraction : MonoBehaviour
{
    public int maxRefractions = 10; // Maximum number of refractions
    public float rayDistance = 10f; // Distance each ray travels
    public LayerMask waterLayer; // Layer mask for water colliders
    public Color rayColor = Color.yellow; // Color for visualizing the ray

    [SerializeField] private GLLineDrawer lineDrawer;
    [SerializeField] private float angleChange;


    void Update()
    {
        SimulateLightRay();
    }

    private void SimulateLightRay()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentDirection = (new Vector3(-6,-4,0) - transform.position).normalized; // Start direction is to the right
        int refractionCount = 0;

        while (refractionCount < maxRefractions)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, rayDistance);
            Vector3 endPosition = hit.collider ? (Vector3)hit.point : currentPosition + currentDirection * rayDistance;

            // Draw the ray in the Game View
            Debug.DrawLine(currentPosition, endPosition, rayColor);

            if (hit.collider)
            {
                // If hit something, check if it's water
                if (((1 << hit.collider.gameObject.layer) & waterLayer) != 0)
                {
                    // If it's water, refract and cast a new ray
                    currentDirection = Quaternion.Euler(0, 0, -angleChange) * currentDirection;
                    currentPosition = hit.point; // Start from the hit point
                    refractionCount++;
                }
                else
                {
                    // If it's not water, stop the ray
                    break;
                }
            }
            else
            {
                // If no hit, end the simulation
                break;
            }
        }
    }
}