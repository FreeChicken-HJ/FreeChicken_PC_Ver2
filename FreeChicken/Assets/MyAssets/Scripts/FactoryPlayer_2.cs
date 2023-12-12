using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class FactoryPlayer_2 : MonoBehaviour
{
    [Header("Settings")]
    public Animator anim;
    public float speed = 2.5f;
    public float hAxis;
    public float vAxis;
    public float jumpPower;
    Vector3 moveVec;
    Rigidbody rigid;
    public GameObject thisRealObj;

    

    [Header("Bool")]
    public bool isJump;
    public bool isSlide;
    public bool isEbutton;
    public bool isDie;
    public bool isStamp;
    public bool isTalk;
    public bool isStopSlide; 
    public bool isContact;
    public bool isLoading;
    public bool isSavePoint;
    public bool isEasy;
    public bool isUnActive;
    [Header("Stats")]
    public GameObject stampTmp;
    public GameObject dieCanvas;
    public GameObject diePs;
    public FactorySceneChangeZone changeZone;
    public ParticleSystem jumpPs;
    public GameObject pickUpPs;

    public GameObject spawnPos;
    public GameObject savePoint;

    [Header("UI")]
    public GameObject scene2LastUI;
    
    public GameObject gameManager;
    public GameManager_Easy gameManager_Easy;
    public GameManager_Hard gameManager_Hard;
    public GameObject memCountUI;
    public GameObject savePointTxt;
    [Header("Camera")]
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera dieCam;
    public CinemachineVirtualCamera pickUpCam;

    [Header("Audio")]
    public AudioSource jumpAudio;
    public AudioSource BGM;
    public AudioSource dieAudio;
    public AudioSource changeConAudio;
    public AudioSource savePointAudio;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isTalk = false;
        changeZone = GameObject.Find("ChangeConveyorZone").GetComponent<FactorySceneChangeZone>();
        BGM.Play();
        MemoryCount.memCount = 2;
        if (isEasy)
        {
            gameManager_Easy = gameManager.GetComponent<GameManager_Easy>();
        }
        else if (!isEasy)
        {
            gameManager_Hard = gameManager.GetComponent<GameManager_Hard>();
        }
    }
   
    void Update()
    { 

        if (!isUnActive && !isTalk && !isDie)
        {
            Move();
            GetInput();
            Turn();
            Jump();
            
        }
        if (changeZone.isButton && isUnActive)
        {
            anim.SetBool("isWalk", false);
        }
        if (isStamp)
        {
            this.gameObject.transform.position = stampTmp.transform.position;
        }
      
    }
    void PickUP()
    {

        dieCanvas.SetActive(true);
        isDie = true;
        dieAudio.Play();
        Invoke("ExitCanvas", 1.5f);
    }

   
    public void GetInput()
    {

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

    }

    void Move()
    {


        if (!(hAxis == 0 && vAxis == 0))
        {
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            transform.position += moveVec * speed * Time.deltaTime * 1f;
            anim.SetBool("isWalk", true);


        }
        else if (hAxis == 0 && vAxis == 0)
        {
            anim.SetBool("isWalk", false);
        }


    }
    void Turn()
    {
        transform.LookAt(transform.position + moveVec); 
    }
    public void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (!isJump)
            {
                isJump = true;
                jumpAudio.Play();
                jumpPs.Play();
                anim.SetTrigger("doJump");
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            }

        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sense") && !isStamp)
        {
            stampTmp = collision.gameObject;
            pickUpCam.Priority = 100;
            mainCam.Priority = 1;
            isSlide = false;
            isStamp = true;
            thisRealObj.gameObject.transform.localScale = new Vector3(2f, 0.5f, 2f);
            pickUpPs.SetActive(true);
            Invoke("PickUP", 2f);

        }

        if (collision.gameObject.CompareTag("PickUpPoc") && !isStamp)
        {
            stampTmp = collision.gameObject;
            pickUpCam.Priority = 100;
            mainCam.Priority = 1;
            isSlide = false;
            isStamp = true;
            pickUpPs.SetActive(true);
            Invoke("PickUP", 2f);

        }
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Slide") || collision.gameObject.CompareTag("EggBox") 
            || collision.gameObject.CompareTag("Item"))
        {

            isJump = false;
        }
        if (collision.gameObject.CompareTag("ObstacleZone2") || collision.gameObject.CompareTag("Obstacle") &&!isDie)
        {
            isDie = true;
            diePs.SetActive(true);
            anim.SetTrigger("doDie");
            dieAudio.Play();
            anim.SetBool("isDie", true);
            dieCanvas.SetActive(true);
            mainCam.Priority = 1;
            dieCam.Priority = 2;
            Invoke("ExitCanvas", 2f);
        }
       

    }

    public void ExitCanvas()
    {
       
        DeadCount.count++;
        if (isStamp)
        {
            isStamp = false;
            pickUpPs.SetActive(false);
            pickUpCam.Priority = -1;
            thisRealObj.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        }         
        diePs.SetActive(false);
        dieCanvas.SetActive(false);
        memCountUI.SetActive(false);
        anim.SetBool("isDie", false);
        
        isSlide = true;      
        isDie = false;
        
        Pos();
        dieCam.Priority = 1;
        mainCam.Priority = 2;
    }
    void Pos()
    {
        if (isEasy && isSavePoint)
        {
            this.transform.position = savePoint.transform.position;
        }
        else
        {
            this.gameObject.transform.position = spawnPos.transform.position;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Rail")
        {
            if (isEasy)
            {
                if (MemoryCount.memCount == 3)
                {
                    scene2LastUI.gameObject.SetActive(true);
                    Invoke("RoadScene", 2f);
                }
                else if (MemoryCount.memCount < 3)
                {
                    memCountUI.gameObject.SetActive(true);
                    Invoke("ExitCanvas", 1.5f);
                }
            }
            else
            {
                if (MemoryCount.memCount == 4)
                {
                    scene2LastUI.gameObject.SetActive(true);
                    Invoke("RoadScene", 2f);
                }
                else if (MemoryCount.memCount < 4)
                {
                    memCountUI.gameObject.SetActive(true);
                    Invoke("ExitCanvas", 1.5f);
                }
            }
        }
        if (isEasy && other.CompareTag("SavePoint_1"))
        {
            isSavePoint = true;
            savePointAudio.Play();
            savePoint.SetActive(false);
            savePointTxt.SetActive(true);
            Invoke("DestroySavePointTxt", 2f);
        }
    
    }
    void DestroySavePointTxt()
    {
        savePointTxt.SetActive(false);
    }
    void RoadScene()
    {
        if (isEasy)
        {
          
            LoadingSceneManager.LoadScene("FactoryScene_3_Easy");
        }
        else
        {

            LoadingSceneManager.LoadScene("FactoryScene_3_Hard");
        }

    }
   
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Slide"))
        {

            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 4f, Space.World);
      
        }
        if (other.CompareTag("TurnPointR"))
        {

            this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * 1f, Space.World);

        }
        if (other.CompareTag("TurnPointL"))
        {

            this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * 1f, Space.World);

        }
        if (other.CompareTag("TurnPointD"))
        {

            this.gameObject.transform.Translate(Vector3.back * Time.deltaTime * 1f, Space.World);

        }
        if (isEasy && other.CompareTag("Item"))
        {
            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 10f, Space.World);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slide"))
        {
            isSlide = false;
        }

       
    }


}
