using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollowPl : MonoBehaviour
{
    public Transform player;  // Assign the player in the Inspector
    public Vector3 offset = new Vector3(0, 2, -10); // Adjust the offset
    public float followSpeed = 5f; // Speed of camera movement

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
