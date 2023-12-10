using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FactoryFirstManager : MonoBehaviour
{
    [Header("Settings")]
    public Animator anim;
    public FactoryPlayer player;
    public GameObject[] boxPos;   
    public GameObject particle;  
    public GameObject talkCanvas1;
    public GameObject talkCanvas2; 
    public GameObject attackBox;
    public GameObject Wall;
    public GameObject eggBoxSpawnPos;
    public GameObject eggBox;
   
    [Header("Camera")]
    public CinemachineVirtualCamera mainCam;  
    public CinemachineVirtualCamera managerCam;
    public CinemachineVirtualCamera managerInCam;
   
    [Header("Audio")]
    public AudioSource hitAudio;
    public AudioSource mainAudio_1;
    public AudioSource mainAudio_2;
    public AudioSource heartAudio;
   
    [Header("Bool")]
    public bool isContact;
    public bool isChk;
    public bool isDie;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("FactoryPlayer").GetComponent<FactoryPlayer>();
      
    }

   /* void Update()
    {
        if (isContact)
        {
            StartCoroutine(Spawn());
            
        }
    }*/
    IEnumerator Spawn()
    {
        isContact = false;
        Vector3 pos = GetRandomPos();

        if (player.tmpBox.transform.position == pos)
        {

            yield return new WaitForSeconds(2f);

            talkCanvas1.SetActive(true);
            managerInCam.Priority = 2;
            managerCam.Priority = 1;
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(Die());

        }
        else
        {

            yield return new WaitForSeconds(1.5f);
            talkCanvas2.SetActive(true);
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(Turn());
        }       
      
    }
    IEnumerator Die()
    {
        isDie = true;
        talkCanvas1.SetActive(false);
        SetPlayerAct();
        if (!Wall.activeSelf) Wall.SetActive(true);     
        SetEggBoxPos();
        SetAudioandCam();
        DeadCount.count++;
      
        yield break;
    }
    void SetAudioandCam()
    {
        if (isDie)
        {
            managerInCam.Priority = 1;
            mainCam.Priority = 2;        
            mainAudio_1.Play();
        }
        else
        {
            managerCam.Priority = 1;
            managerInCam.Priority = -1;
            mainCam.Priority = 2;
            mainAudio_2.Play();
        }
        heartAudio.Stop();
    }
    void SetEggBoxPos()
    {
        
        Vector3 pos = eggBoxSpawnPos.transform.position;
        Quaternion rotate = new Quaternion(-0.0188433286f, -0.706855774f, -0.706855536f, 0.0188433584f);
        eggBox.transform.SetPositionAndRotation(pos, rotate);
        eggBox.GetComponent<FactoryMoveEggBox>().isChk = false;
    }
    void SetPlayerAct()
    {
        player.eggPrefab.SetActive(false);
        player.thisMesh.SetActive(true);
        player.isEgg = false;
        anim.SetBool("isAttack", false);
        if (isDie)
        {
            player.isSetEggFinish = false;
            player.isClick = false;
            player.Pos();

            isChk = false;
            isDie = false;
        }
        else
        {
            eggBox.GetComponent<FactoryMoveEggBox>().Speed = 0.1f;
            player.isStopSlide = false;

        }

    }
    IEnumerator Turn()
    {
        talkCanvas2.SetActive(false);
        SetPlayerAct();
        SetAudioandCam();
        yield break;
    }
    
    Vector3 GetRandomPos()
    {
   
        Vector3 pos = attackBox.transform.position;       
        return pos;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EggBox") && !isChk)
        {           
            isContact = true;
            isChk = true;
            eggBox.GetComponent<FactoryMoveEggBox>().Speed = 0f;
            anim.SetBool("isAttack", true);
            Invoke("PlayHitSound", .5f);
            StartCoroutine(Spawn());
        }
        
    }
    void PlayHitSound()
    {
        hitAudio.Play();
    }
}
