using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefab;
    public int poolSize = 5;

    private List<GameObject> objectPool;
    GameObject go;
    void Start()
    {
        int selection = Random.Range(0, prefab.Length);
        go = prefab[selection];
        InitializeObjectPool();
    }

    void InitializeObjectPool()
    {
        objectPool = new List<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(go, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        
        GameObject newObj = Instantiate(go, position, rotation);
        objectPool.Add(newObj);
        return newObj;
    }
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
      
    }
}
