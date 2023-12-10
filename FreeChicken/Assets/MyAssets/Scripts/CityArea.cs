using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityArea : MonoBehaviour
{
    public float destroyDisatance;
    public CitySceneSpawn citySceneSpawn;
    public CityScenePlayer playerTransform;
    public bool isSpawn;

    public void Setup(CitySceneSpawn citySceneSpawn, CityScenePlayer playerTransform)
    {
        this.citySceneSpawn = citySceneSpawn;
        this.playerTransform = playerTransform;
    }

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<CityScenePlayer>();
    }

    void Update()
    {
        if (playerTransform.transform.position.z - this.transform.position.z >= destroyDisatance && !citySceneSpawn.isStop)
        {
           
            citySceneSpawn.SpawnArea();
            isSpawn = true;           
            gameObject.SetActive(false);
        }
    }
}
