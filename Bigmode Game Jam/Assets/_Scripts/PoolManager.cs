using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Class that can be used for public references to any object pool if it is added to the dictionary
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [SerializeField] private GameObject explosionPrefab;
    private bool explosivesPresent = false; 
    private Dictionary<string, ObjectPool> poolIdentifier;
    private void Awake()
    {
        poolIdentifier = new Dictionary<string, ObjectPool>();
        var newPool = gameObject.AddComponent<ObjectPool>();
        newPool.GeneratePool(15, explosionPrefab);
        AddPool("Explosives", newPool);
        if (instance == null)
        {
            instance = this;
        }
    }
    public void AddPool(string name, ObjectPool pool)
    {
        poolIdentifier.Add(name, pool);
    }
    public ObjectPool GetPool(string name)
    {
        return poolIdentifier[name];
    }
    public bool CheckPool(string name)
    {
        return poolIdentifier.ContainsKey(name);
    }
    public GameObject GetItemFromPool(string name)
    {
        var pool = poolIdentifier[name];
        return pool.RequestAndReturnToPool();
    }
    public void RegisterExplosive()
    {
        explosivesPresent = true;
    }
}
