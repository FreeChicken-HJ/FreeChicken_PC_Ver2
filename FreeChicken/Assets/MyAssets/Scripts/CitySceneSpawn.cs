using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySceneSpawn : MonoBehaviour
{
    public GameObject[] areaPrefabs;
    public GameObject lastMap;
    public float zDistance;
    int areaIndex = 0;
    int spawnAreaCntStart = 2;
    public CityScenePlayer currentPlayer;
  

    public bool isStop;
    public bool isFinish;
  
    public Material[] newSkyBox;
    private List<GameObject> areaPool;
    void Awake()
    {
        areaPool = new List<GameObject>();
        for(int i = 0; i < spawnAreaCntStart; i++)
        {
            
            if (i == 0)
            {
                SpawnArea(false);
            }
            else
            {
                SpawnArea();
            }
        }
    }
    void Start()
    {
        currentPlayer = GameObject.FindWithTag("Player").GetComponent<CityScenePlayer>();
        
    }
    private void Update()
    {
        if (areaIndex == 5 && !isFinish)
        {
            isStop = true;
            SpawnLastMap();
           
        }
    }
    public void SpawnArea(bool isRandom = true)
    {      
        GameObject clone = GetPooledArea(isRandom);
        if(clone == null) { return; }
        if(isRandom == true)
        {
            int ranSkyRange = Random.Range(0, newSkyBox.Length);
            RenderSettings.skybox = newSkyBox[ranSkyRange];
        }
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, areaIndex * zDistance);
        clone.GetComponent<CityArea>().Setup(this,currentPlayer);
        
        areaIndex++;       
    }
    GameObject GetPooledArea(bool isRandom)
    {
        foreach (GameObject obj in areaPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }        
        int ranRange = isRandom ? Random.Range(0, areaPrefabs.Length) : 0;
        GameObject newObj = Instantiate(areaPrefabs[ranRange]);
        areaPool.Add(newObj);
        return newObj;
    }
    void SpawnLastMap()
    {
        isFinish = true;
        GameObject clone = null;
        clone = Instantiate(lastMap);
        
        clone.transform.position = new Vector3(0, 0, areaIndex * zDistance);
        
    }
}
