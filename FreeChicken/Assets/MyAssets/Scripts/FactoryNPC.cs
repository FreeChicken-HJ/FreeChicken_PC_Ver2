using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class FactoryNPC : MonoBehaviour
{
    [Header("Settings")]  
    public FactoryPlayer player;
    public FactoryPlayer_2 player_2;
    public FactoryPlayer_3 player_3;
    public HouseScenePlayer player_4;
    public GameObject npc;
    public GameObject camImage;
    public AudioSource getMemorySound;
    
    [Header("Bool")]
    public bool isEbutton;  
    public bool isNear;
    public static bool isFinish;
    public bool isSet;
    public float t;
    public bool isEasyVer;
    public bool isFactoryScene_1;
    
    [Header("Camera")]   
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera npcCam;
    
    [Header("UI")] 
    public TextMeshProUGUI E;
    public GameObject englishUI;
    public GameObject koreanUI;
    public GameObject ebutton;  
    public Slider npcUI;
    public GameObject factoryUI;
    public GameObject getMemoryUI;
   
    
    void Start()
    {
        
        ebutton.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<FactoryPlayer>();
        player_2 = GameObject.FindWithTag("Player").GetComponent<FactoryPlayer_2>();
        player_3 = GameObject.FindWithTag("Player").GetComponent<FactoryPlayer_3>();
        
        t = 0;
       
    }
  
    void Update()
    {
        
        if (isEbutton && Input.GetButton("E"))
        {
           
            E.color = Color.red;           
            if (npcUI.value <100f)
            {
                t += Time.deltaTime;
                npcUI.value = Mathf.Lerp(0,100,t);
            }
            else
            {
                camImage.SetActive(true);
                isEbutton = false;
                npc.SetActive(false);
                
                if (player != null)
                {
                    player.isStopSlide = true;
                    player.isSlide = false;
                    player.isTalk = true;
                }
                else if(player_2 != null)
                {
                    player_2.isStopSlide = true;
                    player_2.isSlide = false;
                    player_2.isTalk = true;
                }
                else if (player_3 != null)
                {
                    player_3.isStopSlide = true;
                    player_3.isSlide = false;
                    player_3.isTalk = true;
                   
                }
                
                getMemorySound.Play();
                getMemoryUI.SetActive(true);
               
                ebutton.SetActive(false);

                MemoryCount.memCount++;
                
                Invoke("ReStart", 2f);


            }
        }
        if (Input.GetButtonUp("E"))
        {
            E.color = Color.white;
            t = 0;
            npcUI.value = 0;
        }
        
    }
   void ReStart()
    {
        
        camImage.SetActive(false);
        getMemoryUI.SetActive(false);
        isFinish = true;
       
        this.gameObject.SetActive(false);
       
        if (factoryUI != null)
        {
            factoryUI.gameObject.SetActive(true);
            if (PlayerData.isEnglish )
            {
                englishUI.SetActive(true);
            }
            else if (!PlayerData.isEnglish)
            {
                koreanUI.SetActive(true);
            }
        }
        else if(factoryUI == null)
        {
            if (isEasyVer && isFactoryScene_1) 
            {
                
                LoadingSceneManager.LoadScene("FactoryScene_2_Easy");
            }
            else if (isFactoryScene_1)
            {

                LoadingSceneManager.LoadScene("FactoryScene_2_Hard");
            }
            else
            {
                isNear = false;
                isEbutton = false;
                ebutton.SetActive(false);
                mainCam.Priority = 2;
                npcCam.Priority = 1;
                if (player != null)
                {
                    player.isTalk = false;
                }
                else if (player_2 != null)
                {
                    player_2.isTalk = false;
                }
                else if (player_3 != null)
                {
                    player_3.isTalk = false;

                }
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isNear = true;
            isEbutton = true;

            ebutton.SetActive(true);
            mainCam.Priority = 1;
            npcCam.Priority = 2;
        }
    }
   
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            isEbutton = false;
            
            ebutton.SetActive(false);
            mainCam.Priority = 2;
            npcCam.Priority = 1;
            
        }
    }

}
