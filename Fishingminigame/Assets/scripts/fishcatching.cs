using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishCatching : MonoBehaviour
{
    public Slider slider;
    public float increaseRate = 0.3f;
    public float decayRate = 0.8f;

    void Start()
    {

    }

    void Update()
    {
        if (slider != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                slider.value += increaseRate * Time.deltaTime;
            }
            else
            {
                slider.value -= decayRate * Time.deltaTime;
            }

            slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);
        }
    }
}
