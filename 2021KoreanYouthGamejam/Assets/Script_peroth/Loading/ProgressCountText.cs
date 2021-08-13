using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class ProgressCountText : MonoBehaviour
    {
        [SerializeField] private Text text;

        private void Update()
        {
            text.text = $"{Mathf.Ceil(100 * SceneLoadManager.instance.progressBarFillAmount)}%";
        }
    }
}