using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class LoadingText : MonoBehaviour
    {
        [SerializeField] private Text text;
        private readonly float changeDelay = 0.006f;
        private readonly byte changeValue = 1;
        private bool isUp;


        // Start is called before the first frame update
        void OnEnable()
        {
            isUp = false;
            StartCoroutine(LoadingTextCoroutine());
            StartCoroutine(ChangeTextColorCoroutine());
        }

        private IEnumerator LoadingTextCoroutine()
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