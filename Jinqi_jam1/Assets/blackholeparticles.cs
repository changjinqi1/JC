using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackholeparticles : MonoBehaviour
{
    private ParticleSystem particleSystem;

    public float spawnRadius = 25f;

    public float particleSpeed = 5f;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();

        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle; 
        shape.radius = spawnRadius;  

        var main = particleSystem.main;
        main.startSpeed = 0f;

        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.radial = -particleSpeed;
    }

    void Update()
    {
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }
}