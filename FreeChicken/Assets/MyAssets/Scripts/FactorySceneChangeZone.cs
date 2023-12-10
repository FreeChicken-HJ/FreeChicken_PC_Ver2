using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
public class FactorySceneChangeZone : MonoBehaviour
{
    [Header("Stats")]
    public GameObject changeConveorZone;
    public GameObject changeFinish;
    public FactoryPlayer_2 player;
    public ParticleSystem ps;
    public AudioSource particleSound;

    public GameObject zoneL;
    public GameObject zoneR;
    public GameObject zoneG;
    public float t;

    public GameObject bigEgg;
    //public GameObject pos;

    public AudioSource clickSound;

    public TextMeshProUGUI rTxt;
    public TextMeshProUGUI eTxt;
    public ObjectPool objectPool;
    public GameObject bigEggPos;
    [Header("Bool")]
    public bool isButton;
    public bool isL;
    public bool isR;
    public bool isG;
   
    public bool isScene_2;
    public bool isChk;
    public bool isEnd;
   
    [Header("UI")]
    public Slider changeConveorSlider;
  

    [Header("Camera")]
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera changeCam;

   
    void Update()
    {
        if (isButton)
        {
            Chk();
        }
        if(player.isDie)
        {

            StartCoroutine("Reset");
          
        }
    }
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1.7f);
        zoneL.SetActive(false);
        zoneR.SetActive(false);
        zoneG.SetActive(false);
        changeConveorZone.SetActive(false);
        
        changeConveorSlider.value = 0;
        t = 0;
        
        isEnd = false;      
        isButton = false;
        isChk = false;
        
        changeCam.Priority = 1;
        mainCam.Priority = 3;
    }
    void Chk()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isL && !isG && !isR)
        {
            SetRButtonClick();
            zoneR.SetActive(false);
            zoneL.SetActive(true);
            isL = true;
            isR = false;
            isG = false;
        }
       
        else if (Input.GetKeyDown(KeyCode.R) && !isG && isL && !isR)
        {
            SetRButtonClick();
            zoneL.SetActive(false);
            zoneG.SetActive(true);
            isL = false;
            isG = true;
            isR = false;
        }
        
        else if (Input.GetKeyDown(KeyCode.R) && !isR && !isL && isG)
        {
            SetRButtonClick();
            zoneG.SetActive(false);
            zoneR.SetActive(true);
            isG = false;
            isR = true;
            isL = false;
            
        } 
        isR = false;
       
        if (Input.GetKeyUp(KeyCode.R))
        {
            rTxt.color = Color.white;
        }

        if (Input.GetButton("E"))
        {
            
            eTxt.color = Color.red;
            if (changeConveorSlider.value < 100f)
            {
                t += Time.deltaTime;
                changeConveorSlider.value = Mathf.Lerp(0, 100, t);
            }
            else
            {
                eTxt.color = Color.white;
                particleSound.Play();
                changeConveorZone.SetActive(false);
                changeFinish.SetActive(true);
                isButton = false;


                player.isSlide = true;
                
               
                StartCoroutine(TheEnd());

            }

        }
        if (Input.GetButtonUp("E"))
        {
            eTxt.color = Color.white;
            t = 0;
            changeConveorSlider.value = 0;
        }

    }
    void SetRButtonClick()
    {
        rTxt.color = Color.red;
        ps.Play();
        clickSound.Play();
    }
    IEnumerator TheEnd()
    {
        yield return new WaitForSeconds(1f);
        changeCam.Priority = 1;
        mainCam.Priority = 3;
        changeFinish.SetActive(false);
        isChk = false;
        isEnd = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isChk && !isEnd)
        {
            isChk = true;
            isButton = true;
            changeConveorZone.SetActive(true);
           
            changeCam.Priority = 3;
            mainCam.Priority = 1;
            StartCoroutine(SpawnBigEgg());
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChk = false;
            changeConveorZone.SetActive(false);
            isButton = false;
            changeCam.Priority = 1;
            mainCam.Priority = 3;
        }
    }
    IEnumerator SpawnBigEgg()
    {
        yield return new WaitForSeconds(2.5f);
        GameObject obj = objectPool.GetObjectFromPool(bigEggPos.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5f);
        objectPool.ReturnObjectToPool(obj);
    }
   


}
