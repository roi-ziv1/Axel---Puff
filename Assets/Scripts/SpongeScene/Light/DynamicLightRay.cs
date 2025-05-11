using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
// public class LightRefraction : MonoBehaviour
// {
    // public int maxRefractions = 10; // Maximum number of refractions
    // public float rayDistance = 20f; // Maximum distance the ray can travel
    // public LayerMask waterLayer; // Layer mask for the water layer
    // public float offsetDistance = 0.5f; // Offset distance to position the next ray start point
    //
    // public float airRefractiveIndex = 1.0f; // Refractive index of air
    // public float waterRefractiveIndex = 1.33f; // Refractive index of water
    //
    // private LineRenderer lineRenderer;
    //
    // void Start()
    // {
    //     lineRenderer = GetComponent<LineRenderer>();
    // }
    //
    // void Update()
    // {
    //     SimulateLightRay();
    // }
    //
    // private void SimulateLightRay()
    // {
    //     Vector3 currentPosition = transform.position;
    //     Vector3 currentDirection = (lineRenderer.GetPosition(1)- lineRenderer.GetPosition(0)).normalized; // Start direction is to the right
    //     lineRenderer.positionCount = 1;
    //     lineRenderer.SetPosition(0, currentPosition);
    //
    //     int refractionCount = 0;
    //     float currentRefractiveIndex = airRefractiveIndex;
    //
    //     while (refractionCount < maxRefractions)
    //     {
    //         RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, rayDistance,waterLayer);
    //
    //         if (hit.collider != null)
    //         {
    //             // Add the hit point to the LineRenderer
    //             lineRenderer.positionCount++;
    //             lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
    //
    //             // Determine the refractive index for the next medium
    //             float nextRefractiveIndex = ((1 << hit.collider.gameObject.layer) & waterLayer) != 0
    //                 ? waterRefractiveIndex
    //                 : airRefractiveIndex;
    //
    //             // Compute the refraction direction using Snell's Law
    //             Vector3 hitNormal = hit.normal;
    //             currentDirection = CalculateRefraction(currentDirection, hitNormal, currentRefractiveIndex, nextRefractiveIndex);
    //
    //             // Offset the starting position of the next ray along the hit normal
    //             currentPosition = (Vector3)hit.point + hitNormal * offsetDistance;
    //
    //             // Update the current refractive index
    //             currentRefractiveIndex = nextRefractiveIndex;
    //
    //             refractionCount++;
    //         }
    //         else
    //         {
    //             // If no collider is hit, draw the remaining ray to its maximum length
    //             lineRenderer.positionCount++;
    //             lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition + currentDirection * rayDistance);
    //             break;
    //         }
    //     }
    // }
    
    // private Vector3 CalculateRefraction(Vector3 incident, Vector3 normal, float n1, float n2)
    // {
    //     incident = incident.normalized;
    //     normal = normal.normalized;
    //
    //     // Calculate the cosine of the angle of incidence
    //     float cosTheta1 = Mathf.Clamp(Vector3.Dot(-incident, normal), -1f, 1f);
    //
    //     // Handle total internal reflection
    //     if (cosTheta1 < 0f)
    //     {
    //         // Flip the normal and adjust cosTheta1
    //         normal = -normal;
    //         cosTheta1 = -cosTheta1;
    //     }
    //
    //     float ratio = n1 / n2;
    //     float sinTheta2Squared = ratio * ratio * (1f - cosTheta1 * cosTheta1);
    //
    //     // If sin^2(theta2) > 1, total internal reflection occurs
    //     if (sinTheta2Squared > 1f)
    //     {
    //         // Return reflected direction
    //         return Vector3.Reflect(incident, normal);
    //     }
    //
    //     float cosTheta2 = Mathf.Sqrt(1f - sinTheta2Squared);
    //
    //     // Calculate the refracted direction
    //     return ratio * incident + (ratio * cosTheta1 - cosTheta2) * normal;
    // }

    // public float initialAngle = 0f;  // Initial angle of the line in degrees
    // public float bendAngle = 20f;    // Angle by which the line bends upon contact
    // public float lineLength = 10f;   // Total length of the line
    // public LayerMask bendLayer;      // Layer of the refracting object
    //
    // private LineRenderer lineRenderer;
    //
    // void Start()
    // {
    //     lineRenderer = GetComponent<LineRenderer>();
    //     lineRenderer.positionCount = 2;
    //     DrawLine();
    // }
    //
    // void DrawLine()
    // {
    //     Vector3 currentPosition = transform.position;
    //     Vector3 direction = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).normalized;
    //
    //     lineRenderer.SetPosition(0, transform.position);  // Set the initial position
    //
    //     // Cast a ray to check for contact with the bending object
    //     RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, lineLength, bendLayer);
    //     
    //     if (hit.collider != null)
    //     {
    //         // Set the point of contact as the bending point
    //         Vector3 contactPoint = hit.point;
    //         lineRenderer.SetPosition(1, contactPoint);
    //
    //         // Calculate the new direction with the bend angle
    //         direction = Quaternion.Euler(0, 0, bendAngle) * direction;
    //
    //         // Set the end position after bending
    //         Vector3 endPoint = contactPoint + direction * (lineLength - Vector3.Distance(currentPosition, contactPoint));
    //         lineRenderer.SetPosition(2, endPoint);
    //     }
    //     else
    //     {
    //         // If no contact, just draw a straight line
    //         Vector3 endPoint = currentPosition + direction * lineLength;
    //         lineRenderer.SetPosition(1, endPoint);
    //         lineRenderer.positionCount = 2;  // Only two points needed for a straight line
    //     }
    // }
    //
    // void Update()
    // {
    //     DrawLine();
    // }
// }
    
public class DynamicLightRay : MonoBehaviour
{
    public int maxRefractions = 10; // Maximum number of refractions
    public float rayDistance = 20f; // Distance each ray travels
    public LayerMask waterLayer; // Layer mask for water colliders
    public float angleChange = 45f; // Angle to refract when hitting water

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        SimulateLightRay();
    }

    private void SimulateLightRay()
    {
        Vector3 currentPosition = transform.position;
        Vector3 currentDirection = (new Vector3(-6, -4, 0) - transform.position).normalized;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, currentPosition);

        int refractionCount = 0;

        while (refractionCount < maxRefractions)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, currentDirection, rayDistance);
            Vector3 endPosition = hit.collider ? (Vector3)hit.point : currentPosition + currentDirection * rayDistance;

            // Add the ray's endpoint to the LineRenderer
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPosition);

            if (hit.collider)
            {
                // If it hits a collider, check its tag
                if (hit.collider.CompareTag("Door"))
                {
                    // Deactivate the game object
                    hit.collider.gameObject.SetActive(false);
                    break; // Stop the ray when a door is hit
                }

                // If it hits water, calculate refraction
                if (((1 << hit.collider.gameObject.layer) & waterLayer) != 0)
                {
                    currentDirection = Quaternion.Euler(0, 0, -angleChange) * currentDirection;
                    currentPosition = (Vector3)hit.point + (Vector3)hit.normal * 0.1f; // Slight offset to avoid overlap
                    refractionCount++;
                }
                else
                {
                    // Stop if hitting a non-water object
                    break;
                }
            }
            else
            {
                // Stop if no hit
                break;
            }
        }
    }

}

