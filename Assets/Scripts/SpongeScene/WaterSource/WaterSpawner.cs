using System;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private float waterXSize;
    [SerializeField] private float waterYSize;
    
    
    void Start()
    {
        // turn off the water object
        water.SetActive(false);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            SpawnWater();
        }
    }
    
    private void SpawnWater()
    {
        water.SetActive(true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        
    }
    
}