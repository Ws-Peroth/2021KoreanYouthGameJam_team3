using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace peroth
{
    public class loadingBar : MonoBehaviour
    {
        [SerializeField] Slider slider;
        // Update is called once per frame
        void Update()
        {
            slider.value = SceneLoadManager.instance.progressBarFillAmount;
        }
    }
}