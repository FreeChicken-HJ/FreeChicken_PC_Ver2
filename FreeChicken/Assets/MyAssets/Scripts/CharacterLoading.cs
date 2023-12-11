using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterLoading : MonoBehaviour
{
    public float movementSpeed = 1f; // 조절 가능한 이동 속도
    private Vector3 targetPosition;
    private Vector3 maxPosition = new Vector3(1.99000001f, 2.5f, 0f);

    public void UpdateCharacterMovement(float progress)
    {
        // Limit the movement distance based on the distance to the target position
        float maxMoveDistance = Vector3.Distance(transform.position, targetPosition);
        float moveDistance = Mathf.Min(movementSpeed * Time.deltaTime, maxMoveDistance);

        // Move the character towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDistance);
    }

    public void SetDestination(Vector3 position)
    {
        if (position.x > maxPosition.x || position.y > maxPosition.y || position.z > maxPosition.z)
        {
            targetPosition = maxPosition;
        }
        else
        {
            targetPosition = position;
        }
    }

    public bool HasReachedDestination()
    {
        float distanceToDestination = Vector3.Distance(transform.position, targetPosition);
        return distanceToDestination < 0.01f;
    }

    public Vector3 GetMaxPosition()
    {
        return maxPosition;
    }
}
