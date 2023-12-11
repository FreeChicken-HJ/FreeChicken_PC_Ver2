using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLoading : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Transform destination;

    public void UpdateCharacterMovement(float progress)
    {
        MoveTowardsDestination(progress);
    }

    public void SetDestination(Vector3 position)
    {
        destination.position = position;
    }

    void MoveTowardsDestination(float progress)
    {
        Vector3 direction = destination.position - transform.position;
        float distance = direction.magnitude;
        Vector3 moveDirection = direction.normalized;

        float moveDistance = progress * distance;

        transform.position += moveDirection * moveDistance * Time.deltaTime;
    }
}
