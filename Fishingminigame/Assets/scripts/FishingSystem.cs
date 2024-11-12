using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishingSystem : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject sliderBar;
    public GameObject hitText;

    [Header("Fishing Objects")]
    public GameObject fishingFloat;
    public GameObject landingPoint;

    [Header("Audio")]
    public AudioSource biteAudio;
    public AudioSource hookAudio;

    [Header("QTE Elements")]
    public GameObject qteFrame;
    public GameObject progressBar;
    public GameObject qteBar;
    public GameObject successText;

    [Header("Fish Prefabs")]
    public GameObject[] fishPrefabs;

    [Header("Camera Shake Settings")]
    public Transform cameraTransform;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    private bool isSliding;
    private Slider slider;
    private float landingDistance;
    private bool isCollidingWithFish = false;
    private GameObject currentFish;
    private Vector3 originalCameraPosition; // reset the camera's position after shaking

    private void Start()
    {
        if (hitText != null)
        {
            hitText.SetActive(false);
        }
        successText.SetActive(false);
        qteFrame.SetActive(false);
        progressBar.SetActive(false);

        slider = sliderBar.GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("SliderBar is missing a Slider component.");
            return;
        }

        // Store the original camera position
        originalCameraPosition = cameraTransform.position;

        StartCoroutine(PrepStage());
    }

    private IEnumerator PrepStage()
    {
        sliderBar.SetActive(true);
        fishingFloat.SetActive(false);
        landingPoint.SetActive(false);

        slider.value = 0f;
        isSliding = true;
        float oscillationSpeed = 1.5f;
        float timeElapsed = 0f;

        while (isSliding)
        {
            if (Input.GetMouseButton(0))
            {
                timeElapsed += Time.deltaTime * oscillationSpeed;
                slider.value = Mathf.PingPong(timeElapsed, 1f);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isSliding = false;
                sliderBar.SetActive(false);

                landingDistance = Mathf.Lerp(0.5f, 4.5f, slider.value);
                StartCoroutine(ThrowFishingFloat());
            }
            yield return null;
        }
    }

    private IEnumerator ThrowFishingFloat()
    {
        Vector2 playerPos = transform.position;
        Vector2 targetPos = playerPos + Vector2.right * landingDistance;

        fishingFloat.SetActive(true);
        StartCoroutine(MoveInParabola(fishingFloat.transform, playerPos, targetPos, 1f));

        yield return new WaitUntil(() => Vector2.Distance(fishingFloat.transform.position, targetPos) < 0.1f);

        landingPoint.transform.position = targetPos;
        landingPoint.SetActive(true);

        Collider2D waterCheck = Physics2D.OverlapCircle(targetPos, 0.2f, LayerMask.GetMask("Water"));
        if (waterCheck != null && waterCheck.CompareTag("water"))
        {
            StartCoroutine(WaitingStage());
        }
        else
        {
            StartCoroutine(ReturnFishingFloat());
        }
    }

    private IEnumerator WaitingStage()
    {
        float waitTime = Random.Range(2f, 8f);
        float timeElapsed = 0f;

        float amplitude = 0.1f;
        float frequency = 5f;

        Vector3 originalPosition = fishingFloat.transform.position;

        while (timeElapsed < waitTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                fishingFloat.transform.position = originalPosition;
                yield return StartCoroutine(ReturnFishingFloat());
                StartCoroutine(PrepStage());
                yield break;
            }

            float newY = originalPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
            fishingFloat.transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fishingFloat.transform.position = originalPosition;
        StartCoroutine(BitingStage());
    }

    private IEnumerator BitingStage()
    {
        if (hitText != null)
        {
            hitText.SetActive(true);
        }

        biteAudio.loop = true;
        biteAudio.Play();

        StartCoroutine(ShakeCamera()); // Start camera shake when bite audio starts

        float biteDuration = 0.8f;
        float timeElapsed = 0f;

        while (timeElapsed < biteDuration)
        {
            if (Input.GetMouseButtonDown(0))
            {
                biteAudio.Stop();
                hookAudio.PlayOneShot(hookAudio.clip);

                if (hitText != null)
                {
                    hitText.SetActive(false);
                }

                yield return StartCoroutine(QTEStage());
                yield break;
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (hitText != null)
        {
            hitText.SetActive(false);
        }
        biteAudio.Stop();

        yield return StartCoroutine(WaitingStage());
    }

    private IEnumerator ShakeCamera()
    {
        while (biteAudio.isPlaying)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            cameraTransform.position = originalCameraPosition + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(shakeDuration);
        }

        // Reset camera position after shaking
        cameraTransform.position = originalCameraPosition;
    }

    private IEnumerator ReturnFishingFloat()
    {
        Vector2 playerPos = transform.position;
        Vector2 floatPos = fishingFloat.transform.position;

        StartCoroutine(MoveInParabola(fishingFloat.transform, floatPos, playerPos, 1f));

        yield return new WaitUntil(() => Vector2.Distance(fishingFloat.transform.position, playerPos) < 0.1f);

        fishingFloat.SetActive(false);
        landingPoint.SetActive(false);

        StartCoroutine(PrepStage());
    }

    private IEnumerator QTEStage()
    {
        qteFrame.SetActive(true);
        successText.SetActive(false);
        progressBar.SetActive(true);

        Slider progressSlider = progressBar.GetComponent<Slider>();
        if (progressSlider == null)
        {
            Debug.LogError("ProgressBar GameObject is missing a Slider component.");
            yield break;
        }

        // Instantiate a random fish from the array
        if (fishPrefabs.Length > 0)
        {
            Vector2 initialFishPosition = new Vector2(-4.35f, -1.867f);
            GameObject randomFishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
            currentFish = Instantiate(randomFishPrefab, initialFishPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No fish prefabs assigned.");
            yield break;
        }

        // Initialize progress bar
        progressSlider.value = 0.3f;

        float decreaseRate = 0.03f;
        float increaseRate = 0.01f;
        bool qteActive = true;

        while (qteActive)
        {
            progressSlider.value -= Time.deltaTime * decreaseRate;

            if (isCollidingWithFish)
            {
                progressSlider.value += Time.deltaTime * increaseRate;
            }

            if (progressSlider.value >= 0.99f)
            {
                qteActive = false;
                successText.SetActive(true);
                yield return new WaitForSeconds(1f);
                successText.SetActive(false);
                qteFrame.SetActive(false);
                progressBar.SetActive(false);

                Destroy(currentFish);
                StartCoroutine(PrepStage());
                yield break;
            }

            if (progressSlider.value <= 0f)
            {
                qteActive = false;
                qteFrame.SetActive(false);
                progressBar.SetActive(false);

                Destroy(currentFish);
                StartCoroutine(ReturnFishingFloat());
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == currentFish)
        {
            isCollidingWithFish = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentFish)
        {
            isCollidingWithFish = false;
        }
    }

    private IEnumerator MoveInParabola(Transform obj, Vector2 start, Vector2 end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            float height = 2 * (0.5f - Mathf.Abs(0.5f - t));
            obj.position = Vector2.Lerp(start, end, t) + Vector2.up * height;
            time += Time.deltaTime;
            yield return null;
        }
        obj.position = end;
    }
}
