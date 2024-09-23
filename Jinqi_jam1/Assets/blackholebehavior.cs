using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackholebehavior : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Transform player;

    public float outerRadius = 20f;
    public float innerRadius = 10f;
    public float outerForce = 5f;
    public float innerForce = 15f;

    private Rigidbody2D playerRb;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
       
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= outerRadius)
        {
            float forceMagnitude = (distanceToPlayer <= innerRadius) ? innerForce : outerForce;

            Vector2 direction = (transform.position - player.position).normalized;

            playerRb.AddForce(direction * forceMagnitude * Time.deltaTime, ForceMode2D.Force);
        }
    }

}
