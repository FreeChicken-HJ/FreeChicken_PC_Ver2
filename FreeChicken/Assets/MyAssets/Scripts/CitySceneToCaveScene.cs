using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.IO;
public class CitySceneToCaveScene : MonoBehaviour
{
    
    public GameObject pos;
    public CityScenePlayer player;
    public bool isContact;
    public bool isMove;
  
    public CinemachineVirtualCamera endCam;
    public AudioSource CarSound;
    public bool isEasy;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<CityScenePlayer>();
        
    }

    
    void Update()
    {
        if (player.isLast)
        {
            
            if (isContact)
            {
                
                endCam.Priority = 2;
                CarSound.Play();
                player.gameObject.transform.position = pos.transform.position;
                player.anim_2.SetBool("isRun",false);
                this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 4f, Space.World);
                
                Invoke("LoadCaveScene", 3f);
                
               
            }
        }

    }
    void LoadCaveScene()
    {

        
        if (isEasy)
        {
            GameSave.EasyLevel = 4;
           
        
            LoadingSceneManager.LoadScene("Enter2DScene_Easy");
        }
        else
        {
            GameSave.HardLevel = 4;
           
            LoadingSceneManager.LoadScene("Enter2DScene_Hard");
        }
     
     
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isContact = true;
        }
    }
}
