using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushplayer : MonoBehaviour
{
    
    public float stretchFactor = 0.1f;
    public float maxStretch = 2.0f;
    public float minCompress = 0.5f;
    public float minPushForce = 1f;
    public float maxPushForce = 10f;
    public float maxHoldTime = 3f;
    public float cooldownDuration = 3f; // Cooldown duration in seconds
    public int maxPushesBeforeCooldown = 3; // Number of pushes before triggering cooldown

    private float holdTime = 0f;
    private Rigidbody2D parentRigidbody;
    private Vector3 originalScale;
    private bool isStretching = false;
    private int pushCount = 0; // Tracks how many times spacebar has been pressed and released
    private bool isOnCooldown = false; // Tracks whether the cooldown is active

    void Start()
    {
        parentRigidbody = GetComponentInParent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isOnCooldown)
        {
            return; 
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isStretching = true;

            Vector3 newScale = transform.localScale;
            newScale.x = Mathf.Clamp(newScale.x + stretchFactor * Time.deltaTime, 1.0f, maxStretch);
            newScale.y = Mathf.Clamp(newScale.y - stretchFactor * Time.deltaTime, minCompress, 1.0f);
            transform.localScale = newScale;

            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isStretching)
        {
            float currentPushForce = Mathf.Lerp(minPushForce, maxPushForce, holdTime / maxHoldTime);
            Vector2 pushDirection = transform.up;

            parentRigidbody.AddForce(pushDirection * currentPushForce, ForceMode2D.Impulse);

            holdTime = 0f;
            isStretching = false;

            transform.localScale = originalScale;

            pushCount++;

            if (pushCount >= maxPushesBeforeCooldown)
            {
                StartCoroutine(StartCooldown());
            }
        }
    }

    IEnumerator StartCooldown()
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(cooldownDuration);

        pushCount = 0;
        isOnCooldown = false;
    }
}