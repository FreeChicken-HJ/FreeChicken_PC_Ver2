using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory_WallColorChange : MonoBehaviour
{
    public FactoryPlayer player;
    public Renderer[] EggBoxColor;
    int ranRange;
    public Renderer thisMat;
    public GameObject AttackBox;
    public FactoryFirstManager manager;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<FactoryPlayer>();
        manager = GameObject.Find("Manager").GetComponent<FactoryFirstManager>();
        thisMat = GetComponent<Renderer>();
        ranRange = Random.Range(0, EggBoxColor.Length);
        StartCoroutine(WallColorSet());
    }
    IEnumerator WallColorSet()
    {
        while (true)
        {
            yield return new WaitUntil(() => player.isWallChagneColor);

            thisMat.material = EggBoxColor[ranRange].material;
            manager.attackBox = EggBoxColor[ranRange].gameObject;
            player.isWallChagneColor = false;
               
        }
    }
}
