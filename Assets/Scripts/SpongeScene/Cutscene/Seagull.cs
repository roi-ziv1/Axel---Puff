using UnityEngine;
using DG.Tweening;

public class Seagull : MonoBehaviour
{
    [SerializeField] private GameObject[] pathObjects;
    private Vector3[] path;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = new Vector3[pathObjects.Length];
        for (int i = 0; i < pathObjects.Length; i++)
        {
            path[i] = pathObjects[i].transform.position;
        }
        
        // call the FlyAway method after 2 seconds
        Invoke(nameof(FlyAway), 2f);
    }
    
    private void FlyAway()
    {
        //over 2 second rotate the transform of the object to 90 degrees up
        transform.DORotate(new Vector3(0, 0, 90), 2f);
        transform.DOPath(path, 1f, PathType.CatmullRom, pathMode: PathMode.Full3D).SetEase(Ease.Linear).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
