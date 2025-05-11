using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SoftBody : MonoBehaviour
{
    private const float splineOffset = 0.3f;

    #region Fields

    [SerializeField] private SpriteShapeController spriteShape;
    [SerializeField] private Transform[] springyBalls; // All 8 springy balls

    private Dictionary<int, Transform> splineToBallMapping;

    #endregion

    #region MonoBehaviourCallbacks

    private void Awake()
    {
        // Initialize the dictionary
        AssignSplineToBalls();
    }

    private void Update()
    {
        UpdateSplinePoints();
    }

    #endregion

    #region PrivateMethods

    private void AssignSplineToBalls()
    {
        splineToBallMapping = new Dictionary<int, Transform>();

        // Iterate through spline points and find the closest ball
        for (int i = 0; i < spriteShape.spline.GetPointCount(); ++i)
        {
            Transform closestBall = null;
            float closestDistance = float.MaxValue;

            foreach (Transform ball in springyBalls)
            {
                float distance = Vector2.Distance(ball.localPosition, spriteShape.spline.GetPosition(i));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBall = ball;
                }
            }

            // Assign the closest ball to the spline point
            if (closestBall != null)
            {
                splineToBallMapping[i] = closestBall;
            }
        }
    }

    private void UpdateSplinePoints()
    {
        // Update spline points to follow the assigned balls
        foreach (var pair in splineToBallMapping)
        {
            int splineIndex = pair.Key;
            Transform ball = pair.Value;

            // Move the spline point to match the ball's position
            spriteShape.spline.SetPosition(splineIndex, ball.localPosition);
        }

        // Ensure the spline remains closed
        spriteShape.spline.isOpenEnded = false;
    }

    #endregion
}
