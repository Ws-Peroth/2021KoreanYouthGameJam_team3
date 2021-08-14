using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace peroth {

    public class SoundControll : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        private void Start()
        {
            slider.value = MusicClass.instance.GetSoundValue() * 10;
        }
        public void SetSoundSize()
        {
            MusicClass.instance.ChangeSoundValue(slider.value / 10);
        }
    }
}