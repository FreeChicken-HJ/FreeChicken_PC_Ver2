using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class CityScenePlayer : MonoBehaviour
{
    [Header("Settings")]
    public GameObject lastZonePlayer;
    public GameObject currentZonePlayer;
    Rigidbody rigid;
    public Animator anim_1;
    public Animator anim_2; 
    public bool isStart;    
    public ParticleSystem jumpPs;
    public ParticleSystem diePs;
    public float jumpPower;
    bool isJump;
    public float hAxis;
    public float vAxis;
    public float speed;
    public GameObject talkUI;
    public GameObject startUI;
    public GameObject gameManager;
    public GameManager_Easy gameManager_Easy;
    public GameManager_Hard gameManager_Hard;
    public GameObject deathUI;
   
    [Header("Bool")]
    public bool isLast;
    public bool isAllStop; 
    public bool isfallingFruits;
    public bool ishurdleUp;
    public bool isChk;
    bool isDie;
    bool particleAttack;
    public bool isEasy;
    [Header("Camera")]
    public GameObject Cam;
    public CinemachineVirtualCamera startCam;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera dieCam;
    public CinemachineVirtualCamera jumpCam;

    [Header("Audio")]
    public AudioSource startAudio;
    public AudioSource bgm;
    public AudioSource dieAudio;
    public AudioSource jumpAudio;
    public AudioSource changeAudio;
    public AudioSource ringAudio;
    
    void Start()
    {
        Cursor.visible = false;
        startAudio.Play();
        rigid = GetComponent<Rigidbody>();
        //diePs.gameObject.SetActive(false);
        particleAttack = false;
        isAllStop = true;
        if (isEasy)
        {
            gameManager_Easy = gameManager.GetComponent<GameManager_Easy>();
            gameManager_Easy.isLoading = true;
        }
        else
        {
            gameManager_Hard = gameManager.GetComponent<GameManager_Hard>();
            gameManager_Hard.isLoading = true;
        }
        
        Invoke("NewStart", 2.9f);
    }
    void NewStart()
    {
        startUI.SetActive(false);
        deathUI.SetActive(true);
        startAudio.Stop();
        bgm.Play();
        if (isEasy)
        {
            gameManager_Easy.isLoading = false;
        }
        else
        {           
            gameManager_Hard.isLoading = false;
        }
        isAllStop = false;
        isStart = true;
        startCam.Priority = -1;
        mainCam.Priority = 1;
    }
    void Update()
    {       
        if (!isDie && !isAllStop)
        {
            Jump();
        }
        if (isAllStop)
        {
            anim_1.SetBool("isRun", false);
        }
        
    }
    void FixedUpdate()
    {
        if (!isDie && !isAllStop)
        {
            GetInput();
            Move();
        }
        if (this.transform.position.y < -5f &&!isAllStop &&!isDie)
        {
            
            TagisObj();
        }
        
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        
    }
    void Move()
    {

        Vector3 position = transform.position;
        position.x += hAxis * Time.smoothDeltaTime * speed;
        if (!isLast)
        {
            position.z += Time.smoothDeltaTime * speed ;
        }
        else if (isLast)
        {
            if (vAxis != 0 || hAxis !=0)
            {
                anim_2.SetBool("isRun", true);
                position.z += vAxis * Time.smoothDeltaTime * speed;
            }
            else if(vAxis == 0)
            {
                anim_2.SetBool("isRun", false);
            }
        }
        transform.position = position;
       
        anim_1.SetBool("isRun", true);
        

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJump)
            {
                
                if (!isLast)
                {
                    jumpCam.Priority = 100;
                    anim_1.SetTrigger("doJump");
                    Invoke("ReSetJumpCam", 1f);
                }
         
                isJump = true;
                jumpAudio.Play();
                rigid.AddForce( Vector3.up* jumpPower, ForceMode.Impulse);               
                jumpPs.Play();
               
            }

        }
       
    }
    void ReSetJumpCam()
    {
        
        jumpCam.Priority = -1;
       
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            
            isJump = false;
        }
        if (collision.gameObject.tag == "Obstacle" && !isDie)
        {
            TagisObj();
        }
        

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Swings")
        {
            rigid.velocity += transform.forward * 3.5f;
        }
    }
    void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Obstacle") && !particleAttack )
        {
            particleAttack = true;
            Destroy(other.gameObject);
            TagisObj();
        }    
    }
    void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Obstacle") &&!isDie)
        {
            TagisObj();
        }
    
        if(other.CompareTag("LastZone") && !isChk)
        {
            isAllStop = true;
            isChk = true;
            bgm.Stop();
            ringAudio.Play();
            //LastSong.Play(); 8.16
            talkUI.SetActive(true);
            Invoke("Exit", 2f);
        }
      
    }
    void Exit()
    {
        talkUI.SetActive(false);
        isAllStop = false;

        isLast = true;
        lastZonePlayer.SetActive(true);
        changeAudio.Play();
        
        currentZonePlayer.SetActive(false);
        jumpPower = 15f;
       

    }
   
    void TagisObj()
    {
        if (!isDie)
        {
            isDie = true;
            dieAudio.Play();
         
            diePs.gameObject.SetActive(true);
            dieCam.Priority = 100;
            anim_1.SetTrigger("doDie");
            anim_1.SetBool("isRun", false);
            rigid.isKinematic = true;
            DeadCount.count++;
            if(!isAllStop) Invoke("ReLoadScene", 1f);
        }
    }
    void ReLoadScene()
    {
        if (isEasy)
        {
            SceneManager.LoadScene("CityScene_Easy");
        }
        else
        {
            SceneManager.LoadScene("CityScene_Hard");
        }
    }
}
