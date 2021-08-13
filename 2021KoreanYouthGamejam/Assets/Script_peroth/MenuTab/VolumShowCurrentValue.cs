using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class VolumShowCurrentValue : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text currentValue;
        [SerializeField] private Text minValueText;
        [SerializeField] private Text maxValueText;
        private int maxValue;

        private int minValue;

        private void Start()
        {
            minValue = (int) slider.minValue;
            maxValue = (int) slider.maxValue;

            minValueText.text = $"{minValue}";
            maxValueText.text = $"{maxValue}";
        }


        private void Update()
        {
            currentValue.text =
                slider.value <= minValue || slider.value >= maxValue ? "" : $"{slider.value}";
        }
    }
}