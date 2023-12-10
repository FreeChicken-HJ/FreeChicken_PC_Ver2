using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
public class FactoryNPC_1 : MonoBehaviour
{
    public Slider NpcUI;
    
    public HouseScene2_Player player;
    public bool isEbutton;
    public GameObject Ebutton;
    public TextMeshProUGUI E;

    public bool isNear;
    
    public CinemachineVirtualCamera npccam;
    public CinemachineVirtualCamera maincam;
    
    public GameObject npc;
    public GameObject Video;

    public AudioSource BGM;
    public AudioSource Memory;
    public GameObject TalkUI1;
    public GameObject TalkUI2;
    public bool isFin;
    public GameObject Wall;
    public GameObject gameManager;
    public GameManager_Easy gameManager_Easy;
    public GameManager_Hard gameManager_Hard;
    float t = 0;
    public bool isEasy;
    void Start()
    {
        Ebutton.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<HouseScene2_Player>();
        if (isEasy)
        {
            gameManager_Easy = gameManager.GetComponent<GameManager_Easy>();
        }
        else if(!isEasy)
        {
            gameManager_Hard = gameManager.GetComponent<GameManager_Hard>();
        }
       
    }
    void Update()
    {
        
        if (Input.GetButton("E") && isEbutton)
        {
            
            E.color = Color.red;
            if (NpcUI.value <100f)
            {
                t += Time.deltaTime;
                NpcUI.value = Mathf.Lerp(0,100,t);
            }
            else
            {
                LoadingCheck();
                isEbutton = false;
                Video.SetActive(true);

                E.color = Color.white;
                player.isTalk1 = true;
                Destroy(Ebutton);
                BGM.Stop();
                Memory.Play();
                
                Cursor.visible = true;
                Invoke("ReStart", 38f);
                isFin = true;
            }
        }
        if (Input.GetButtonUp("E"))
        {
            t = 0;
            E.color = Color.white;
            NpcUI.value = 0;
        }

       
    }
    public void ReStart()
    {
        if (isFin )
        {
            Video.SetActive(false);
            maincam.Priority = 2;
            npccam.Priority = -5;         
            npc.SetActive(false);
            BGM.Play();
            LoadingUnCheck();
            Wall.SetActive(false);
            Memory.Stop();            
            isFin = false;
            if (!PlayerData.isEnglish)
            {
                TalkUI1.SetActive(true);
            }
            else if( PlayerData.isEnglish)
            {
                TalkUI2.SetActive(true);
            }
        }

       
    }
    void LoadingUnCheck()
    {
        if (isEasy)
        {
            gameManager_Easy.isLoading = false;
        }
        else
        {
            gameManager_Hard.isLoading = false;
        }
    }
    public void LoadingCheck()
    {
        if (isEasy)
        {
            gameManager_Easy.isLoading = true;
        }
        else
        {
            gameManager_Hard.isLoading = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            npccam.Priority = 100;
            maincam.Priority = 1;
            Ebutton.SetActive(true);
            isEbutton = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            npccam.Priority = 1;
            maincam.Priority = 10;
            Ebutton.SetActive(false);
            isEbutton = false;
        }
    }
}
