using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject waterMoleculePrefab; // פריפאב של מולקולת מים
    public int poolSize = 100; // כמות המולקולות בבריכה
    private Queue<GameObject> pool;

    void Start()
    {
        pool = new Queue<GameObject>();

        // יצירת הבריכה
        for (int i = 0; i < poolSize; i++)
        {
            GameObject molecule = Instantiate(waterMoleculePrefab);
            molecule.SetActive(false); // להתחיל עם כל המולקולות כבויות
            pool.Enqueue(molecule);
        }
    }

    public GameObject GetMolecule()
    {
        if (pool.Count > 0)
        {
            GameObject molecule = pool.Dequeue();
            molecule.SetActive(true);
            return molecule;
        }
        else
        {
            Debug.LogWarning("No more molecules available in the pool!");
            return null;
        }
    }

    public void ReturnMolecule(GameObject molecule)
    {
        molecule.SetActive(false);
        pool.Enqueue(molecule);
    }
}