using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.IO;
public class FactoryPlayer : MonoBehaviour
{
    [Header("Setting")]
    public GameObject thisMesh;
    public GameObject thisRealObj;
    public Animator anim;
    public float speed = 2.5f;
    public float hAxis;
    public float vAxis;
    public float jumpPower;
    Vector3 moveVec;
    Rigidbody rigid;
    public ParticleSystem jumpPs;

    [Header("Bool")]
    public bool isJump;
    public bool isSlide;
    public bool isEbutton;
    public bool isDie;
    public bool isStamp;

    public bool isTalk;
    public bool isUnActive;
    public bool isEgg;
    public bool isStopSlide;

    public bool isContactStopSlide;

    public bool isContact;
    public bool isSetEggFinish;
    public bool isStart;
    public bool isStopCon;
    public bool isPickUp;

    public bool isSavePointPos_1;
    public bool isSavePointPos_2;

    public bool isPlaying = false;

    public int DeathCount;

    public bool isWallChagneColor;
    public bool isClick;

    public bool isEnglish;
    public bool isEasyVer;

    [Header("UI")]
    public GameObject turnEggCanvas;
    public GameObject changeEggCanvas;
    public GameObject stopSlideCanvas;
    public GameObject tmpBox;
    public TextMeshProUGUI E;
    public TextMeshProUGUI spaceBar;
    public GameObject dieCanvas;
    public GameObject upstairCanvas;
    public GameObject scene2LastUI;
   
    public GameObject savePointTxt;
    
    
    [Header("Stats")]
    public GameObject eggBoxSpawnTriggerBox;
    public GameObject eggBox;
    public GameObject eggBoxSpawnPos;
    public GameObject eggPrefab;

    
    public GameObject diePs;
    public GameObject pickUpPs;
    public Vector3 pos;

    public GameObject tmp;
    public GameObject stampTmp;
   
    

    public GameObject existingSlideObj;
    public GameObject changeSlideObj;

    public GameObject savePointPos_1;
    public GameObject savePointPos_2;

    public GameObject changeEggDoor;

    public GameObject blockWall;
    
   
    [Header("Camera")]
    public CinemachineVirtualCamera mainCam;
   
    public CinemachineVirtualCamera stopConCam;
    public CinemachineVirtualCamera managerCam;
    public CinemachineVirtualCamera dieCam;
    public CinemachineVirtualCamera pickUpCam;
    public CinemachineVirtualCamera eggChangeCam;


    [Header("Audio")]
    public AudioSource runAudio;
    public AudioSource dieAudio;
    public AudioSource jumpAudio;
    public AudioSource savePointAudio;
    public AudioSource eggChangeZoneAudio;
    public AudioSource mainAudio;
    public AudioSource mainAudio_2;
    public AudioSource heartBeatAudio;
    public AudioSource fixAudio;
    public AudioSource PipeAudio;
    public bool isEng;
    public bool isKorean;
    void Awake()
    {
        mainAudio.Play();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isTalk = false;
        Cursor.visible = false;
        MemoryCount.memCount = 0;

        

    }
  
    void Update()
    {
        
        if (!isUnActive && !isTalk && !isEgg && !isDie && !isStart &&!isPickUp &&!isStamp)
        {
            Move();
            GetInput();
            Turn();
            Jump();
            
        }
        if (!isTalk && isEgg && !isUnActive)
        {
            StartCoroutine("Check");
        }
        if (isTalk && isStamp && isUnActive)
        {
            anim.SetBool("isWalk", false);
        }
        

        if (isPickUp)
        {
            this.gameObject.transform.position = tmp.transform.position;
            
        }
        if (isStamp)
        {
            this.gameObject.transform.position = stampTmp.transform.position;
            
        }
       
    }
    void PickUP()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            dieAudio.Play();
        }
        dieCanvas.SetActive(true);
        Invoke("ExitCanvas", 2f);
    }
    IEnumerator Check()
    {
        yield return new WaitForSeconds(1f);


        if (!isSetEggFinish && Input.GetButton("E"))
        {
                       
            isEgg = false;
            isClick = false;
            changeEggCanvas.SetActive(false);
            yield return new WaitForSeconds(.2f);
            eggPrefab.SetActive(false);           
            thisMesh.SetActive(true);
           
        }
      
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

            runAudio.Play();

        }
        else if (hAxis == 0 && vAxis == 0)
        {
            anim.SetBool("isWalk", false);
            runAudio.Stop();
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
                jumpPs.Play();
                jumpAudio.Play();
                isJump = true;
                anim.SetTrigger("doJump");
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);  
                
            }
        }
       
    }
  
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sense") &&!isStamp)
        {
            stampTmp = collision.gameObject;
            pickUpPs.SetActive(true);
            pickUpCam.Priority = 100;
            mainCam.Priority = 1;
            isSlide = false;
            isStamp = true;
            thisRealObj.gameObject.transform.localScale = new Vector3(2f,0.5f,2f);

            Invoke("PickUP", 2f);

        }
        if(collision.gameObject.CompareTag("PickUpPoc") && !isPickUp)
        {
            tmp = collision.gameObject;
            isPickUp = true;
            isSlide = false;
            pickUpCam.Priority = 100;
            mainCam.Priority = 1;
            pickUpPs.SetActive(true);
            Invoke("PickUP", 2f);

        }
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Slide") || collision.gameObject.CompareTag("EggBox"))
        {

            isJump = false;
        }
       
        if(collision.gameObject.CompareTag("Floor"))
        {
            
                upstairCanvas.SetActive(true);
                Invoke("UpstairExit", 2f);
            
        }
        if (collision.gameObject.CompareTag("ObstacleZone1") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (!isDie && !isPickUp)
            {
                isDie = true;
                anim.SetTrigger("doDie");
                anim.SetBool("isDie", true);

                diePs.SetActive(true);
                dieCanvas.SetActive(true);
                dieCam.Priority = 2;
                mainCam.Priority = 1;

                dieAudio.Play();
                
                if (isSetEggFinish)
                {
                    Invoke("ReLoadScene_2", 2f);
                }
                else
                {
                    Invoke("ExitCanvas", 2f);
                }
            }
            
        }
        
    }

    void UpstairExit()
    {
        upstairCanvas.SetActive(false);
    }
    void ExitCanvas()
    {

        DeadCount.count++;
        
        if (isDie)
        {
            dieCanvas.gameObject.SetActive(false);
            
            
            dieCam.Priority = -3;
            isDie = false;
            diePs.SetActive(false);
            anim.SetBool("isDie", false);
        }
        if(isPickUp)
        {   
           
            isPickUp = false;
            pickUpCam.Priority = -5;
            dieCanvas.SetActive(false);
            pickUpPs.SetActive(false);
        }
        if (isStamp)
        {
        
            isStamp = false;
            thisRealObj.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
            pickUpCam.Priority = -5;    
            dieCanvas.SetActive(false);
            pickUpPs.SetActive(false);
        }
       
        mainCam.Priority = 2;
        Pos();

    }
    public void Pos()
    {
        if (isEasyVer)
        {
            if (isSavePointPos_1 && !isSavePointPos_2)
            {
                this.gameObject.transform.position = savePointPos_1.gameObject.transform.position;

            }
            else if (isSavePointPos_2)
            {
                this.gameObject.transform.position = savePointPos_2.gameObject.transform.position;
                blockWall.SetActive(false);
            }
            else
            {
                this.gameObject.transform.position = pos;
            }
        }
        else
        {
            if (isSavePointPos_1)
            {
                this.gameObject.transform.position = savePointPos_1.gameObject.transform.position;
                blockWall.SetActive(false);
            }
            else
            {
                this.gameObject.transform.position = pos;
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
       
        if(other.CompareTag("Rock"))
        {
           
            mainCam.Priority = -1;
            eggChangeCam.Priority = 10;
            blockWall.SetActive(true);
            mainAudio.Stop();
            mainAudio_2.Stop();
            eggChangeZoneAudio.Play();
        }
       
        if (other.CompareTag("StopSlide")) 
        {

            
            mainCam.Priority = 1;
            stopConCam.Priority = 2;
            stopSlideCanvas.gameObject.SetActive(true);
            eggBoxSpawnTriggerBox.SetActive(true);
            existingSlideObj.SetActive(false);
            changeSlideObj.SetActive(true);
            isWallChagneColor = true;


        }
        if(other.CompareTag("EggBoxPos"))
        {
            
            Vector3 pos = eggBoxSpawnPos.transform.position;
            Quaternion rotate = new Quaternion(-0.0188433286f, -0.706855774f, -0.706855536f, 0.0188433584f);
          
            eggBox.SetActive(true);
            eggBox.transform.position = pos;
            eggBox.transform.rotation = rotate;
            
           
        }
        
        if(other.CompareTag("SavePoint_1"))
        {
            isSavePointPos_1 = true;
            savePointAudio.Play();
            savePointPos_1.SetActive(false);
            savePointTxt.SetActive(true);
            Invoke("DestroySavePointTxt", 2f);
        }

        if (other.CompareTag("SavePoint_2") && isEasyVer)
        {
            isSavePointPos_2 = true;
            savePointAudio.Play();
            savePointPos_2.SetActive(false);
            savePointTxt.SetActive(true);
            Invoke("DestroySavePointTxt", 2f);
           
        }
    }
   
    void DestroySavePointTxt()
    {
        savePointTxt.SetActive(false);
    }

    IEnumerator Egg()
    {
        yield return new WaitForSeconds(3f);
        if (!isEgg)  yield break; 
        
        eggChangeCam.Priority = -100;
        changeEggDoor.SetActive(false);
        turnEggCanvas.SetActive(false);
        changeEggCanvas.SetActive(false);

        isSetEggFinish = true;
        eggChangeZoneAudio.Stop();
        heartBeatAudio.Play();
       
    }
    void OnTriggerStay(Collider other)
    {
        if (!isTalk && !isStopSlide && other.CompareTag("Slide"))
        {
            isSlide = true;
        }
        if (!isStopSlide && other.CompareTag("TurnPointR"))
        {
            
            this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * 1f, Space.World);
        }
        if (!isStopSlide && other.CompareTag("TurnPointL"))
        {
            
            this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * 1f, Space.World);
        }
        
        if (!isSetEggFinish &&!isClick && other.CompareTag("PointZone"))
        {
            turnEggCanvas.SetActive(true);
           
            if (Input.GetButton("F"))
            {
                isClick = true;

                Vector3 pos = other.transform.position;

                tmpBox = other.gameObject;
                thisMesh.SetActive(false);
                eggPrefab.SetActive(true);
                turnEggCanvas.SetActive(false);
                changeEggCanvas.SetActive(true);
                
                eggPrefab.transform.position = pos;
               
                isEgg = true;
                
                StartCoroutine(Egg());
            }          
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slide"))
        {
            isSlide = false;
        }
        if(other.CompareTag("PointZone"))
        {
            turnEggCanvas.SetActive(false);
        }
        if(other.CompareTag("StopSlide"))
        {
            
            mainCam.Priority = 2;
            stopConCam.Priority = 1;
            stopSlideCanvas.SetActive(false);
            
        }
        
    }
    
}
