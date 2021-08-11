using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace peroth {
    public class ProgressCountText : MonoBehaviour
    {
        [SerializeField] Text text;
        void Update()
        {
            text.text = $"{Mathf.Ceil(100 * SceneLoadManager.instance.progressBarFillAmount)}%";
        }
    }
}