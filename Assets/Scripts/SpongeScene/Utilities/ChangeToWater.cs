using UnityEngine;

public class ChangeToWater : MonoBehaviour
{
    [SerializeField] private GameObject water;
    

    // Update is called once per frame
    void Update()
    {
        if (water.activeSelf)
        {
            // change layer to water
            gameObject.layer = LayerMask.NameToLayer("Water");
        }
    }
}
