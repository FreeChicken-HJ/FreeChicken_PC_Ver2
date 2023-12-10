using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggDrop : MonoBehaviour
{
    
    public GameObject[] prefab;
    public BoxCollider area;
    public int cnt;
    bool isEggFalltrue;

    public ObjectPool objectPool;
    void Start()
    {
        area = GetComponent<BoxCollider>();
    
    }

    void Update()
    {
        if (isEggFalltrue)
        {

            for (int i = 0; i < cnt; i++)
            {
                StartCoroutine(Spawn());

            }
        }
        isEggFalltrue = false;
       
    }
    IEnumerator Spawn()
    {
        Vector3 pos = GetRandomPos();

        int selection = Random.Range(0, prefab.Length);
        GameObject go = prefab[selection];
        GameObject instance = objectPool.GetObjectFromPool(pos, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        objectPool.ReturnObjectToPool(instance);
       
    }

    Vector3 GetRandomPos()
    {
        Vector3 basePos = transform.position;
        Vector3 size = area.size;

        float posX = basePos.x + Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePos.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePos.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);
        Vector3 pos = area.center + spawnPos;
        return pos;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           
            isEggFalltrue = true;
            
        }      
    }
}
