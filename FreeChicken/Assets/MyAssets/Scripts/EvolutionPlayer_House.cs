using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Cinemachine;
using System.IO;
public class EvloutionPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform characterBody;
    [SerializeField] public Transform cameraArm;
    Vector3 moveVec;
    Vector2 moveInput;
    Rigidbody rigid;
    public float speed;
    public float JumpPower;
    public float hAxis;
    public float vAxis;
    Animator anim;

    [Header("GameObject")]
    public GameObject dieCanvas;
    public GameObject player;
    public ParticleSystem diePs;
    public ParticleSystem jumpPs;
    public GameObject Pos;

    [Header("Bool")]
    bool wDown;
    bool isJump;
    bool isDead;
    public bool isMove;
    public bool isEasy;
    
    [Header("Camera")]
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera unicycleCam;

    [Header("Audio")]
    public AudioSource mainAudio;
    public AudioSource runAudio;
    public AudioSource dieAudio;
    public AudioSource jumpAudio;
    public AudioSource savePointAudio;

    [Header("Dialogue")]
    public GameObject readygoCity;
    public bool isTalk2;
    public bool isTalkEnd2;

    [Header("Evloution")]
    private bool isRotating = false;
    private Quaternion originalCameraRotation;
    private float rotationTimer = 0.0f;
    private float rotationDuration = 2.0f;
    public GameObject EvoluPs;



    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        isJump = false;
    }

    void Start()
    {
        diePs.gameObject.SetActive(false);
        dieCanvas.gameObject.SetActive(false);
        StartCoroutine(CO_notDead());
    }

    IEnumerator CO_notDead()
    {
        while (!isDead)
        {
            if (!isTalk2)
            {
                if (isRotating)
                {
                    HandleCameraRotation();
                }
                else
                {
                    Move();
                    GetInput();
                    Jump();
                    LookAround();

                }
            }
            yield return null;
        }
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
    }

    void Move()
    {
        moveInput = new Vector2(hAxis, vAxis);
        isMove = moveInput.magnitude != 0;

        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            moveVec = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveVec;
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
            runAudio.Play();
        }
        anim.SetBool("Run", isMove);
        anim.SetBool("Walk", wDown);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJump && !isDead)
        {
            jumpAudio.Play();
            isJump = true;
            rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            jumpPs.Play();
        }
    }

    void DieMotion()
    {
        isDead = true;
        dieCanvas.gameObject.SetActive(true);
        diePs.gameObject.SetActive(true);
        anim.SetBool("isDead", true);
        dieAudio.Play();
    }

    void ReLoadScene()
    {
        isDead = false;
        anim.SetBool("isDead",false);
        diePs.gameObject.SetActive(false);
        this.gameObject.transform.position = Pos.gameObject.transform.position;
        dieCanvas.gameObject.SetActive(false);
    }
  
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("evolu")) 
        {
            StartRotation();
            readygoCity.SetActive(true);
        }

        if (other.gameObject.name == "GoCitySense")
        {
           
            if (isEasy)
            {
                GameSave.EasyLevel = 3;

                LoadingSceneManager.LoadScene("Enter2DScene_Easy");
            }
            else
            {
                GameSave.HardLevel = 3;
                LoadingSceneManager.LoadScene("Enter2DScene_Hard");
            }
        }
    }

    private void HandleCameraRotation()
    {
        rotationTimer += Time.deltaTime;

        // 회전 각도 계산 (0에서 720도까지)
        float rotationAngle = Mathf.Lerp(0f, 720f, rotationTimer / rotationDuration); // 0부터 720도까지 두 바퀴 회전

        // 회전
        cameraArm.RotateAround(transform.position, Vector3.up, rotationAngle * Time.deltaTime);

        EvoluPs.SetActive(true);

        if (rotationTimer >= rotationDuration)
        {
            rotationTimer = 0.0f;
            isRotating = false;

            // 회전이 완료된 후에 원래 상태로 돌아가는 처리 추가
            cameraArm.rotation = originalCameraRotation;
            EvoluPs.SetActive(false);
        }
    }

    public void StartRotation()
    {
        isRotating = true;
        originalCameraRotation = cameraArm.rotation; 
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("evolu"))
        {
            other.gameObject.SetActive(false);
            readygoCity.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("House2_Obstacle") && !isDead)
        {
            isDead = true;
            DieMotion();
            dieCanvas.gameObject.SetActive(true);
            Invoke("ReLoadScene", 3.5f);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    public void LookAround() // 카메라
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

