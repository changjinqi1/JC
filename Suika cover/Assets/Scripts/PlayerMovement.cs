using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private bool isCollidingWithLwall = false;
    private bool isCollidingWithRwall = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(moveX, 0f);

        if (isCollidingWithLwall && Input.GetKeyDown(KeyCode.A))
        {
            transform.position = new Vector2(-0.3f, 3.38f);
        }

        if (isCollidingWithRwall && Input.GetKeyDown(KeyCode.D))
        {
            transform.position = new Vector2(-3.7f, 3.38f);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lwall"))
        {
            isCollidingWithLwall = true;
            Debug.Log("into Lwall");
        }

        if (collision.gameObject.CompareTag("Rwall"))
        {
            isCollidingWithRwall = true;
            Debug.Log("into Rwall");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lwall"))
        {
            isCollidingWithLwall = false;
            Debug.Log("exit Lwall");
        }

        if (collision.gameObject.CompareTag("Rwall"))
        {
            isCollidingWithRwall = false;
            Debug.Log("exit Rwall");
        }
    }
}
