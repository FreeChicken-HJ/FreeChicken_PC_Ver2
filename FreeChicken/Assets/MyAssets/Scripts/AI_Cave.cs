using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AI_Cave : MonoBehaviour
{
    [SerializeField] private string AIName;
    [SerializeField] private float walkSpeed;

    public LayerMask Ground, Player;
    public bool isAttacked;

    public Transform target;

    public Vector3 pos;

    public bool Small_AI;
    public bool Big_AI;
    public bool Obstacle_AI;

    private bool isWalking;
    private bool isRun;
    private bool isAttack;

    bool isDie;

    public bool playerInSight, playerInAttack;

    public float timeBetweenAttacks;

    public CaveScenePlayer player;

    public GameObject small_potion;
    public GameObject mesh;
    public GameObject key;
    [SerializeField] private Animator anim;
    [SerializeField] Rigidbody rigid;
    [SerializeField] private CapsuleCollider capsuleCol;

    NavMeshAgent nav;

    private bool isCollision = false; // 충돌 상태 확인
    public float hideTime; // 숨겨질 시간
    public float respawnTime; // 다시 생성될 시간
    public GameObject ResetPos1; // 원래 위치
    public GameObject ResetPos2;
    public bool AI_Cave1;
    public bool AI_Cave2;

    public AudioSource keyDropSound;
    public AudioSource DieSound;
    public AudioSource AttackSound;
    public GameObject DieParticle_1;
    public GameObject DieParticle_2;
    
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.Find("CaveCharacter").GetComponent<CaveScenePlayer>();
        Big_AI = true;
    }

    void Update()
    {
        if (!isDie )
        {
            nav.isStopped = false;
            playerInSight = Physics.CheckSphere(transform.position, 5f, Player);
            playerInAttack = Physics.CheckSphere(transform.position, 1f, Player);
            if (Small_AI && !Big_AI && !playerInAttack) MoveRandom();
            if (!Small_AI && Big_AI &&playerInSight && !playerInAttack) Targeting();
        }

        if (Small_AI && !Big_AI)
        {
            this.gameObject.tag = "Slide";
        }
        if (Big_AI && !Small_AI)
        {
            this.gameObject.tag = "Obstacle";
        }
        if (!isWalking && !isRun && isAttack)
        {

            anim.SetTrigger("doAttack");
            anim.SetBool("isAttack", true);
            anim.SetBool("Running", false);
        }
        if (!isWalking && isRun && !isAttack)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("isAttack", false);
            anim.SetBool("Running", true);
        }
        if (isWalking && !isRun && !isAttack)
        {
            anim.SetBool("Running", false);
            anim.SetBool("Walking", true);
            anim.SetBool("isAttack", false);

        }
        if (player.Dead)
        {
            isWalking = true;
            isRun = false;
        }
    }

    void Targeting() // 캐릭터 발견 & 따라가기 
    {

        isAttack = false;
        isRun = true;
        isWalking = false;
        nav.SetDestination(target.position);
    }
    void MoveRandom() //랜덤이동 하다가 
    {
        isAttack = false;
        isRun = true;
        isWalking = false;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            if (!nav.pathPending && nav.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Vector3 newTarget = FindNewTarget();
                if (newTarget != Vector3.zero)
                {
                    nav.SetDestination(newTarget);
                }
            }
            else if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
            {

                Vector3 newTarget = FindNewTarget();
                nav.SetDestination(newTarget);
            }
        }
    }

    private Vector3 FindNewTarget()
    {
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }

    void Attack1()
    {
        Invoke("ResetAttack1", respawnTime);
    }

    void ResetAttack1()
    {
        nav.SetDestination(ResetPos1.gameObject.transform.position);
        nav.isStopped = true;
        isCollision = false;
    }

    void Attack2()
    {
        this.gameObject.SetActive(false);
        Invoke("ResetAttack2", respawnTime);
    }

    void ResetAttack2()
    {
        this.gameObject.transform.position = ResetPos2.gameObject.transform.position;
        this.gameObject.SetActive(true);    
        isCollision = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Small_Potion") && !Small_AI && Big_AI)
        {
            Small_AI = true;
            Big_AI = false;
            small_potion.SetActive(false);
            DieSound.Play();
            this.gameObject.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        }

        if(collision.gameObject.CompareTag("Player") && AI_Cave1 && !AI_Cave2)
        {
            if (!isCollision)
            {
                isCollision = true;
                Invoke("Attack1", hideTime);
            }
        }

        if(collision.gameObject.CompareTag("Player") && !AI_Cave1 && AI_Cave2)
        {
            if(!isCollision)
            {
                isCollision = true;
                Invoke("Attack2", hideTime);
            }
        }

        if(collision.gameObject.CompareTag("Obstacle") && !isDie)
        {
            isDie = true;
            anim.SetTrigger("isDead");
            DieParticle_2.SetActive(true);
            rigid.isKinematic = true;
            DieSound.Play();

            Collider AICollider = GetComponent<Collider>();
            AICollider.enabled = false;

            if (AI_Cave2 && isDie)
            {   
                this.gameObject.tag = "Slide";
                Invoke("DestroyAI_Obstacle", 3f);
            }
        }
    }

    void DestroyAI_Cave()
    {
        keyDropSound.Play();
        mesh.SetActive(false);
        key.SetActive(true);
        DieParticle_1.SetActive(false);
    }

    void DestroyAI_Obstacle()
    {
        DieParticle_2.SetActive(false);
        this.gameObject.SetActive(false);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FootPos" && !isDie && Small_AI && !Big_AI)
        {
            isDie = true;
            DieParticle_1.SetActive(true);
            AttackSound.Play();
            anim.SetTrigger("isDead");
            Invoke("DestroyAI_Cave", 2f);
        }
    }
}
