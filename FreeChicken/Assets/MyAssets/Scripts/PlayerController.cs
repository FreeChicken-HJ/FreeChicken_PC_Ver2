using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;
//using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    public bool isFin;  
    public bool isGo; 
    public Animator anim;
 
    public GameObject Pos;
    public GameObject ChickFam;
    public CinemachineVirtualCamera Firststart;
    public CinemachineVirtualCamera start;
    public CinemachineVirtualCamera main;
    public CinemachineVirtualCamera End;
    public CinemachineVirtualCamera EndStart;

    public AudioSource BGM;
    public AudioSource BGM2;
    public AudioSource ChickSound;
    public AudioSource PipeSound;
    public AudioSource bgm3;
    public GameObject BlackImage;
    public GameObject targetPosition;
    public GameObject StartUI;
    void Awake()
    {
        anim = GetComponent<Animator>();
        Firststart.Priority = 2;
    }
    void Start()
    {
        
        Invoke("FamGO", 3f);
        Invoke("StartCam", .5f);
        BGM.Play();
        ChickSound.Play();
        
    }

    void Update()
    {
      
        
        if (isGo && !isFin)
        {
            ChickFam.transform.position -= new Vector3(Pos.transform.position.x, ChickFam.transform.position.y, ChickFam.transform.position.z) * Time.deltaTime * .01f;
            anim.SetBool("Walk", true);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, .01f);

        }

    }
    void StartCam()
    {
        Firststart.Priority = 0;
        start.Priority = 2;
    }
   void FamGO()
    {
        main.Priority = 3;
        isGo = true;
    }
  
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            BGM.Stop();
            ChickSound.Stop();
            BlackImage.SetActive(true);
            Invoke("BlackOut", 1f);
            BGM2.Play();
            EndStart.Priority = 11;
            this.gameObject.SetActive(false);
            Invoke("LastCam", 2.5f);
        }
       
    }
    void BlackOut()
    {
        BlackImage.SetActive(false);
    }
    void LastCam()
    {
        if (!isFin)
        {
            BlackImage.SetActive(true);

            Invoke("BlackOut_2", 2f);
        }
    }
    void BlackOut_2()
    {
        if (!isFin)
        {
            BlackImage.SetActive(false);
            PipeSound.Play();
            End.Priority = 12;
            Invoke("StartTalkScene", 4f);
        }
    }
    void StartTalkScene()
    {
        if (!isFin)
        {
            StartUI.SetActive(true);
            BGM2.Stop();
            PipeSound.Stop();
            bgm3.Play();
        }
    }
    public void ClickSikp()
    {
        isFin = true;
        StartUI.SetActive(true);
        BGM.Stop();
        ChickSound.Stop();
        BGM2.Stop();
        PipeSound.Stop();
        bgm3.Play();
    }
}
