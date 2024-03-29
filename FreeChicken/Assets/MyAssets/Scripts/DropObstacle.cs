using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObstacle : MonoBehaviour
{
    public ObjectPool objectPool;
    public GameObject[] prefab;
    BoxCollider area;
    public int cnt = 20;
    CaveScenePlayer player;

    void Start()
    {
        area = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CaveScenePlayer>();
    }

    void Update()
    {
        if (player.isfallingObstacle)
        {
            for (int i = 0; i < cnt; ++i)
            {
                StartCoroutine(Spawn());
            }
        }
        player.isfallingObstacle = false;
    }

    IEnumerator Spawn()
    {
        Vector3 pos = GetRandomPos();
        int selection = Random.Range(0, prefab.Length);
        GameObject go = prefab[selection];
        //GameObject instance = Instantiate(go, pos, Quaternion.identity);
        float random = Random.Range(3.5f, 5f);
       //Destroy(instance, random);

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
}
