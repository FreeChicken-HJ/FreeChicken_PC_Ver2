using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Cinemachine;
using System.ComponentModel;
//using UnityEngine.UIElements;

public class HouseScene2_Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform characterBody;
    [SerializeField] public Transform cameraArm;
    Vector2 moveInput;
    Rigidbody rigid;
    public float speed;
    public float jumpPower;
    public float hAxis;
    public float vAxis;
    Animator anim;

    [Header("Bool")]
    public bool isMove;
    bool wDown;
    public bool isJump;
    public bool isFallingObstacle;
    bool isDead;
    public bool isUnActive;

    [Header("GameObject")]
    public GameObject player;
    public GameObject dieCanvas;
    public ParticleSystem DiePs;
    public ParticleSystem JumpPs;
    public GameObject Pos;
    public GameObject Pos2;
    public GameObject evolutionPlayer;
    public GameObject evolutionSense;
    public GameObject evoluPs;
    public GameObject backyardDoor;
    public Vector3 ResPawnPos1;
    public Vector3 ResPawnPos2;
    public GameObject DestroyObj;
    public GameObject UnicycleObj;
    public GameObject LineObj;
    public GameObject portal;
    public GameObject npcPs;

    [Header("Camera")]
    public CinemachineVirtualCamera npc_cam;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera unicycleCam;

    [Header("Audio")]
    public AudioSource mainAudio;
    public AudioSource runAudio;
    public AudioSource dieAudio;
    public AudioSource jumpAudio;
    public AudioSource savePointAudio;
    public AudioSource trumpetAudio;
    public AudioSource duckAudio;
    public AudioSource windAudio;

    [Header("Dialogue")]
    public GameObject npcDialogue1;
    public GameObject npcDialogue2;
    public GameObject npc;
    public GameObject unicycleDialogue1;
    public GameObject unicycleDialogue2;
    public bool isTalk1;
    public bool isTalkEnd1;
    public bool isTalk2;
    public bool isTalkEnd2;

    [Header("Evloution")]
    private bool isRotating = false;
    private Quaternion originalCameraRotation;
    private float rotationTimer = 0.0f;
    private float rotationDuration = 3.0f;
    
    void Awake()
    {
        mainAudio.Play();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        isJump = false;
    }

    void Start()
    {
        Cursor.visible = false;
        DiePs.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isDead && !isUnActive)
        {
            if (!isTalk1 || !isTalk2)
            {
                if (isRotating)
                {
                    HandleCameraRotation();
                }
                else
                {
                    Move();
                    LookAround();
                    GetInput();
                    Jump();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDead && !isUnActive)
        {
            hAxis = 0;
            vAxis = 0;
            wDown = false;
        }
        if (this.transform.position.y < -3f && !isDead)
        {
            DieMotion();
        }
    }

    void GetInput()
    {
        if (!isDead)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");
            wDown = Input.GetButton("Walk");
        }
    }

    void Move()
    {
        if (!isTalk1 && !isTalk2 && !isDead)
        {
            moveInput = new Vector2(hAxis, vAxis);
            isMove = moveInput.magnitude != 0;

            if (isMove)
            {
                Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
                Vector3 moveVec = lookForward * moveInput.y + lookRight * moveInput.x;

                characterBody.forward = moveVec;
                rigid.MovePosition(transform.position + moveVec * speed * Time.deltaTime);
                runAudio.Play();
            }
            anim.SetBool("Run", isMove);
            anim.SetBool("Walk", wDown);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump && !isDead)
        {
            jumpAudio.Play();
            isJump = true;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            JumpPs.Play();
        }
    }

    void DieMotion()
    {
        if (isDead)
        {
            DiePs.gameObject.SetActive(true);
            dieCanvas.gameObject.SetActive(true);          
            anim.SetTrigger("doDead");         
            dieAudio.Play();
            Invoke("ReLoadScene", 2f);
        }
    }

    void ReLoadScene()
    {
        DeadCount.count++;
        if (isTalkEnd1)
        {
            rigid.MovePosition(ResPawnPos1);
        }
        else 
        {
            rigid.MovePosition(ResPawnPos2);
        }
        DiePs.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(false);
          
        isDead = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropBox"))
        {
            isFallingObstacle = true;
        }
        if (!isDead && other.CompareTag("House2_Obstacle"))
        {
            isDead = true;
            DieMotion();
        }
        if (other.gameObject.name == "Portal")
        {
            portal.SetActive(false);
            windAudio.Play();
        }
        if (other.gameObject.CompareTag("NPC") && !isTalk1 && !isTalkEnd1 && !PlayerData.isEnglish)
        {
            isTalk1 = true;
            npcDialogue1.SetActive(true);
            npcPs.SetActive(false);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            backyardDoor.SetActive(false);
            npc_cam.Priority = 10;
            mainCam.Priority = 1;
        }
        if (other.gameObject.CompareTag("NPC") && !isTalk1 && !isTalkEnd1 && PlayerData.isEnglish)
        {
            isTalk1 = true;
            npcDialogue2.SetActive(true);
            npcPs.SetActive(false);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            backyardDoor.SetActive(false);
            npc_cam.Priority = 10;
            mainCam.Priority = 1;
        }
        if (other.gameObject.name == "Unicycle_Sense" && !isTalk2 && !isTalkEnd2 && !PlayerData.isEnglish)
        {
            isTalk2 = true;
            unicycleDialogue1.SetActive(true);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            DestroyObj.SetActive(false);
            LineObj.SetActive(true);
            unicycleCam.Priority = 10;
            mainCam.Priority = 1;
            Invoke("UnicycleObj_Destroy", 1.5f);
        }
        if (other.gameObject.name == "Unicycle_Sense" && !isTalk2 && !isTalkEnd2 && PlayerData.isEnglish)
        {
            isTalk2 = true;
            unicycleDialogue2.SetActive(true);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            DestroyObj.SetActive(false);
            LineObj.SetActive(true);
            unicycleCam.Priority = 10;
            mainCam.Priority = 1;
            Invoke("UnicycleObj_Destroy", 1.5f);
        }
        if (other.gameObject.name == "EvolutionSense1")
        {
            trumpetAudio.Play();
            StartRotation();
            LineObj.SetActive(true);
            Invoke("Destroy_", 2f);
        }
    }

    void Destroy_()
    {
        Destroy(this.gameObject);
        
        evolutionPlayer.SetActive(true);
        evolutionSense.SetActive(false);
    }

    private void HandleCameraRotation()
    {
        rotationTimer += Time.deltaTime;

        float rotationAngle = Mathf.Lerp(0f, 720f, rotationTimer / rotationDuration);

        if (rotationAngle != 0f)  // 회전이 발생했을 때만 처리
        {
            cameraArm.RotateAround(transform.position, Vector3.up, rotationAngle * Time.deltaTime);
            evoluPs.SetActive(true);
        }
        if (rotationTimer >= rotationDuration)
        {
            rotationTimer = 0.0f;
            isRotating = false;

            cameraArm.rotation = originalCameraRotation;
            evoluPs.SetActive(false);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
        originalCameraRotation = cameraArm.rotation;
    }

    void UnicycleObj_Destroy()
    {
        UnicycleObj.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npc_cam.Priority = 1;
            mainCam.Priority = 10;

            isTalkEnd1 = true;
        }
        if (other.gameObject.name == "Unicycle_Sense")
        {
            isTalkEnd2 = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDead && collision.gameObject.CompareTag("House2_Obstacle"))
        {

            isDead = true;
            DieMotion();
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }

        if (collision.gameObject.name == "RubberDuck")
        {
            duckAudio.Play();
        }
    }

    public void LookAround() 
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 100f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}