using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVehicle : MonoBehaviour
{
    public GameObject car;
    public GameObject destroyPos;
    Vector3 curPosition;
    Vector3 curRotation;
    Quaternion rotation;
    public ObjectPool objectPool;
   
    void Start()
    {
        curPosition = transform.position;
        curRotation = transform.localEulerAngles;
        rotation = Quaternion.Euler(curRotation);


        StartCoroutine(Spawn());
        
    }
   
 
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            GameObject obj = objectPool.GetObjectFromPool(curPosition, rotation);
            yield return new WaitForSeconds(2f);
            objectPool.ReturnObjectToPool(obj);
           
        }

    }
  
}
