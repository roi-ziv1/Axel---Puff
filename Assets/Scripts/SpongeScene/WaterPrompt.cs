using UnityEngine;

public class WaterPrompt : MonoBehaviour
{
    [SerializeField] private GameObject waterPrompt;
    
    void Start()
    {
        waterPrompt.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            waterPrompt.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            waterPrompt.SetActive(false);
        }
    }
}
