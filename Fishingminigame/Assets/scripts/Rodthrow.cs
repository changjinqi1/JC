using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rodthrow : MonoBehaviour
{
    public GameObject fishingFloat; // Reference to the fishing float GameObject
    public GameObject landingPoint; // Reference to the landing point GameObject
    public Slider slider; // Reference to the slider UI element
    public TextMeshProUGUI hitText; // Reference to the hit text UI element
    public AudioSource bitingAudioSource; // Reference to the audio source for biting audio
    public AudioSource hookedAudioSource; // Reference to the audio source for hooked audio
    public float minLandingDistance = 0.5f;
    public float maxLandingDistance = 4.5f;

    private void Start()
    {
        // Ensure fishing float and landing point are deactivated initially
        fishingFloat.SetActive(false);
        landingPoint.SetActive(false);
        slider.gameObject.SetActive(false); // Hide the slider at start
        StartFishing();
    }

    private void StartFishing()
    {
        StartCoroutine(FishingStages());
    }

    private IEnumerator FishingStages()
    {
        // Prep Stage
        PrepStage();
        yield return StartCoroutine(SliderControl());
    }

    private void PrepStage()
    {
        // Ensure fishing float and landing point are deactivated initially
        fishingFloat.SetActive(false);
        landingPoint.SetActive(false);
        slider.value = 0; // Reset the slider to 0
    }

    private IEnumerator SliderControl()
    {
        slider.gameObject.SetActive(true); // Activate the slider when left mouse is pressed
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                // Move slider while the left mouse button is held down
                slider.value = Mathf.PingPong(Time.time * 1.5f, 1); // Move slider from 1 to 0 and back
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // When left mouse button is released, activate the landing point and fishing float
                Vector3 playerPosition = transform.position;
                SetLandingPointPosition(playerPosition);
                ActivateLandingPoint();
                ActivateFishingFloat();
                yield return StartCoroutine(MoveFishingFloat(playerPosition, landingPoint.transform.position));
                break; // Exit the loop after processing
            }
            yield return null; // Wait for the next frame
        }
    }

    private void SetLandingPointPosition(Vector3 playerPosition)
    {
        float sliderValue = slider.value;
        Vector3 landingPointPosition = new Vector3(playerPosition.x + Mathf.Lerp(minLandingDistance, maxLandingDistance, sliderValue), playerPosition.y, playerPosition.z);
        landingPoint.transform.position = landingPointPosition; // Move landing point to the calculated position
        Debug.Log("Landing point position set to: " + landingPoint.transform.position); // Debug the position
    }

    private void ActivateLandingPoint()
    {
        landingPoint.SetActive(true); // Activate the landing point
        Debug.Log("Landing point activated."); // Debug log for landing point activation
    }

    private void ActivateFishingFloat()
    {
        fishingFloat.SetActive(true); // Now activated after slider control is complete
        Debug.Log("Fishing float activated."); // Debug log to check activation
    }

    private IEnumerator MoveFishingFloat(Vector3 start, Vector3 end)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Parabolic movement
            Vector3 midPoint = Vector3.Lerp(start, end, t);
            midPoint.y += Mathf.Sin(t * Mathf.PI) * 2; // Adjust height for parabolic effect
            fishingFloat.transform.position = midPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fishingFloat.transform.position = end; // Ensure final position
    }

    private IEnumerator WaitingStage()
    {
        float waitTime = Random.Range(3f, 5f);
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                // If clicked, move fishing float back to player
                Vector3 playerPosition = transform.position;
                yield return MoveFishingFloat(fishingFloat.transform.position, playerPosition);
                fishingFloat.SetActive(false);
                landingPoint.SetActive(false); // Deactivate landing point
                StartFishing(); // Restart the fishing process
                yield break; // Exit the coroutine
            }

            yield return null; // Wait for the next frame
        }

        // Enter Biting Stage after waiting
        yield return BitingStage();
    }

    private IEnumerator BitingStage()
    {
        hitText.gameObject.SetActive(true); // Show hit text
        bitingAudioSource.Play(); // Start playing biting audio

        float bitingDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < bitingDuration)
        {
            elapsedTime += Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
            {
                // Stop biting audio and play hooked audio
                bitingAudioSource.Stop();
                hookedAudioSource.PlayOneShot(hookedAudioSource.clip); // Play hooked audio once
                yield return QTEStage(); // Enter QTE stage
                yield break; // Exit the coroutine
            }

            yield return null; // Wait for the next frame
        }

        bitingAudioSource.Stop(); // Stop biting audio after duration
        yield return WaitingStage(); // Return to waiting stage
    }

    private IEnumerator QTEStage()
    {
        // Implement QTE stage logic here
        // For now, we'll just wait before going back to prep stage
        yield return new WaitForSeconds(2f); // Wait for 2 seconds (placeholder)
        StartFishing(); // Restart the fishing process
    }
}
