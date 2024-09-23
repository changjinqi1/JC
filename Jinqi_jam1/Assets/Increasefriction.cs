using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increasefriction : MonoBehaviour
{
    private Rigidbody2D rb;
    private PhysicsMaterial2D material;

    public float defaultFriction = 0.4f;

    public float increasedFriction = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        material = new PhysicsMaterial2D();
        material.friction = defaultFriction;
        rb.sharedMaterial = material;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            material.friction = increasedFriction;
        }
        else
        {
            material.friction = defaultFriction;
        }
    }
}