using UnityEngine;

public class FollowFish : MonoBehaviour
{
    // make a script that makes the camera follow the fish object
    public Transform fish;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
    private bool isFollowing = false;
    
    void Start()
    {
        offset = transform.position - fish.position;
        isFollowing = true;
    }
    
    void FixedUpdate()
    {
        if (!isFollowing) return;
        Vector3 desiredPosition = fish.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    
    public void StartFollowing()
    {
    
    }
}
