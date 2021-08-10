using System.Collections;
using System.Collections.Generic;
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

        private int minValue;
        private int maxValue;

        private void Start()
        {
            minValue = (int)slider.minValue;
            maxValue = (int)slider.maxValue;

            minValueText.text = $"{minValue}";
            maxValueText.text = $"{maxValue}";
        }
        
        
        void Update() => 
            currentValue.text = 
            slider.value <= minValue || slider.value >= maxValue ? "" : $"{slider.value}";

    }
}