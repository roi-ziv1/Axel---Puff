using System.Collections;
using SpongeScene.Managers;
using SpongeScene.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

public class ReduceWater : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 10f; // Speed of water reduction
    [SerializeField] private float growSpeed = 3f; // Speed of water reduction
    [SerializeField] private float maxAmount = 8; // Maximum amount of water
    [SerializeField] private float currentAmount = 8; // Current amount of water
    [FormerlySerializedAs("p")] [SerializeField] private WaterPost post;
    private Vector3 initialScale; // Stores the initial scale of the object
    private bool isGrowing = false; // Tracks if the object is currently growing
    private bool isReducing = false; // Tracks if the object is currently reducing
    private Vector3 targetScale = Vector3.zero; // The minimum scale the object can shrink to
    private float waterPercentage;
    private bool touchingPlayer = false;
    private float noPlayerTimer = 2f;
    private float noPlayerTime = 0f;

    void Start()
    {
        // Save the initial scale of the object
        initialScale = transform.localScale;
        UpdatePercentage();
        if (CoreManager.Instance.EventsManager)
        {
            CoreManager.Instance.EventsManager.AddListener(EventNames.Die, ResetWater);
        }
    }

    private void ResetWater(object obj)
    {
        currentAmount = maxAmount;
        UpdatePercentage();
    }
    
    void Update()
    {
        if (touchingPlayer)
        {
            noPlayerTime = 0;
        }
        else
        {
            noPlayerTime += Time.deltaTime;
            if (noPlayerTime >= noPlayerTimer)
            {
                currentAmount = maxAmount;
                UpdatePercentage();
            }
        }
        targetScale = new Vector3(initialScale.x, initialScale.y * waterPercentage, initialScale.z);
        if(targetScale.y > transform.localScale.y)
        {
            isGrowing = true;
        }
        else if(targetScale.y < transform.localScale.y)
        {
            isReducing = true;
        }
        // If the object is reducing
        if (isReducing)
        {
            // Gradually reduce the scale of the object
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, shrinkSpeed * Time.deltaTime);
            // Stop reducing if the scale is close enough to zero
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale; 
                isReducing = false; // Stop reducing
            }
            
        }
        
        // If the object is growing and the growth is gradual
        if (isGrowing)
        {
            if (post && !post.p.isPlaying)
            {
                post.PlayParticles();
            }
            // Gradually increase the scale of the object
            
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growSpeed * Time.deltaTime);
            // Stop growing if the scale is close enough to the target
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale; 
                isGrowing = false; // Stop growing
                if (post)
                {
                    post.StopParticles();

                }
            }
        }
        
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     currentAmount -= 1;
        //     if (currentAmount <= 0)
        //     {
        //         currentAmount = 0.3f;
        //         // Change Layer to Default
        //         gameObject.layer = LayerMask.NameToLayer("Default");
        //     }
        // }
        // else if(Input.GetKeyDown(KeyCode.S))
        // {
        //     if (currentAmount < maxAmount)
        //     {
        //         currentAmount += 1;
        //         if(gameObject.layer != LayerMask.NameToLayer("Water"))
        //         {
        //             gameObject.layer = LayerMask.NameToLayer("Water");
        //         }
        //     }
        // }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = true;
        }
        if (other.gameObject.layer != LayerMask.NameToLayer("Water")) return;
        if (!(currentAmount < maxAmount)) return;
        currentAmount += 1;
        UpdatePercentage();
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
    
    public void Reduce()
    {
        currentAmount -= 1;
        if (currentAmount <= 0)
        {
            currentAmount = 0f;
            // Change Layer to Default
            //
        }
        UpdatePercentage();
        
    }
    
    private void UpdatePercentage()
    {
        float tempPercentage = currentAmount / maxAmount;
        waterPercentage = tempPercentage;
        // if (tempPercentage < 1 && tempPercentage > 0.6)
        // {
        //     waterPercentage = 0.6f;
        // }
        //
        // else if (tempPercentage < 0.6 && tempPercentage > 0.3)
        // {
        //     waterPercentage = 0.3f;
        // }
        // else if (tempPercentage < 0.3)
        // {
        //     waterPercentage = 0.3f;
        // }
        // else if (tempPercentage == 1f)
        // {
        //     waterPercentage = 1;
        // }
        
    }
    
    public bool GetIfEmpty()
    {
        return currentAmount <= 0f;
    }
    
}