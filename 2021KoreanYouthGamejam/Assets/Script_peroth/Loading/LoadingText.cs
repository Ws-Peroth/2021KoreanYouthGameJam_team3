using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class LoadingText : MonoBehaviour
    {
        [SerializeField] private Text text;
        bool isUp;

        [Range(0, 255)]
        [SerializeField] private byte changeValue;

        // Start is called before the first frame update
        void Start()
        {
            isUp = false;
            changeValue = 1;
            StartCoroutine(LoadingTextCoroutine());
        }

        void Update()
        {
            if (!isUp)
                FadeOut();
            else
                FadeIn();
        }

        IEnumerator LoadingTextCoroutine()
        {
            while (true)
            {
                text.text = "Loading";
                yield return new WaitForSeconds(0.1f);

                text.text = "Loading .";
                yield return new WaitForSeconds(0.1f);

                text.text = "Loading . .";
                yield return new WaitForSeconds(0.1f);

                text.text = "Loading . . .";
                yield return new WaitForSeconds(0.1f);
            }
        }

        void FadeIn()
        {
            Color32 color = text.color;
            if (color.a <= 254)
                color.a += changeValue;
            else
            {
                color.a = 255;
                isUp = false;
            }
            text.color = color;
        }

        void FadeOut()
        {
            Color32 color = text.color;
            if (color.a >= 1)
                color.a -= changeValue;
            else
            {
                color.a = 0;
                isUp = true;
            }
            text.color = color;
        }
    }
}