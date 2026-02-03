using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    Queue<GameObject> pool = new Queue<GameObject>();
    GameObject parent;

    public void GeneratePool(int count, GameObject prefab)
    {
        parent = Instantiate(new GameObject("Object Pool"), Vector3.zero, Quaternion.identity); // Instantiate the parent to this group of objects
        for (int i = 0; i < count; i++)
        {
            var temp = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
            temp.SetActive(false);
            pool.Enqueue(temp);
        }
    }
    public GameObject RequestFromPool(GameObject prefab)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
        }
        obj.SetActive(true);
        return obj;
    }
    public GameObject RequestAndReturnToPool(GameObject prefab)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);
        }
        pool.Enqueue(obj);
        obj.SetActive(true);
        return obj;
    }
    public void Enqueue(GameObject obj)
    {
        pool.Enqueue(obj);
    }
}
