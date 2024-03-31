using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private ScriptableVariable max;
    [SerializeField] private ScriptableVariable current;
    private float sliderValue;

    [SerializeField] private Slider slider;

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;

    void Awake()
    {
        slider = this.GetComponent<Slider>();
    }

    void Update()
    {
        sliderValue = current.value / max.value;
        slider.value = sliderValue;
        image.color = ColorFromGradient(sliderValue);
    }

    Color ColorFromGradient(float value)  // float between 0-1
    {
        return gradient.Evaluate(value);
    }
}
