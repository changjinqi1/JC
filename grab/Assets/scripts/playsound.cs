using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsound : MonoBehaviour
{
    public AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gt") || other.CompareTag("rt") ||
            other.CompareTag("gc") || other.CompareTag("rc") ||
            other.CompareTag("trash") || other.CompareTag("poo"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                PlayPickupSound();
            }
        }
    }

    void PlayPickupSound()
    {
        if (pickupSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(pickupSound);
        }
    }
}