using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseObstacle_UpDown : MonoBehaviour
{
    public enum MoveObstacleType { A, B };
    public MoveObstacleType Type;

    public float moveSpeed = 3.0f;
    private bool isPlayerFollow = false;
    private Transform player;
    private Vector3 originalPosition;

    Rigidbody playerRigid;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPosition = transform.position;
        playerRigid = player.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerFollow = true;
            Invoke("DestroyObject", 2f);
            Invoke("RespawnObject", 5f);
        }
    }

    void FixedUpdate()
    {
        switch (Type)
        {
            case MoveObstacleType.A:
                if (isPlayerFollow)
                {
                    transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                    playerRigid.MovePosition(playerRigid.position + Vector3.up * moveSpeed * Time.deltaTime);
                }
                break;

            case MoveObstacleType.B:
                if (isPlayerFollow)
                {
                    transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                    playerRigid.MovePosition(playerRigid.position + Vector3.down * moveSpeed * Time.deltaTime);
                }
                break;
        }
    }

    void DestroyObject()
    {
        gameObject.SetActive(false);
    }

    void RespawnObject()
    {
        gameObject.SetActive(true);
        transform.position = originalPosition;
        isPlayerFollow = false;
    }

}
