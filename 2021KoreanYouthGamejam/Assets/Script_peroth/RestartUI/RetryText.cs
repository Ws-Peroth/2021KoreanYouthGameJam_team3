using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class RetryText : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private float changeDelay = 0.006f;
        [SerializeField] private byte changeValue = 3;
        private bool isUp;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneLoadManager.instance.SceneChange(SceneLoadManager.IntToScenesEnum(StageManager.instance.currentStage));
            }
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            isUp = false;
            StartCoroutine(ChangeTextColorCoroutine());
        }

        private IEnumerator ChangeTextColorCoroutine()
        {
            Color32 color;

            while (true)
            {
                color = text.color;

                if (!isUp)
                {
                    // FADE OUT
                    if (color.a >= 1)
                    {
                        color.a -= changeValue;
                    }
                    else
                    {
                        color.a = 0;
                        isUp = true;
                    }

                    text.color = color;
                }
                else
                {
                    // FADE IN
                    if (color.a <= 254)
                    {
                        color.a += changeValue;
                    }
                    else
                    {
                        color.a = 255;
                        isUp = false;
                    }

                    text.color = color;
                }

                yield return new WaitForSeconds(changeDelay);
            }
        }
    }
}
