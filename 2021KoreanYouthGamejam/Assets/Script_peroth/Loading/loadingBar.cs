using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class loadingBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        // Update is called once per frame
        private void Update()
        {
            slider.value = SceneLoadManager.instance.progressBarFillAmount;
        }
    }
}