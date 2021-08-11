using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace peroth
{
    public enum Scenes
    {
        Openning,
        Main
    }

    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        public static Scenes nextScene;
        public float progressBarFillAmount;

        [SerializeField] private Image progressBar;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(ScenesEnumToInt(nextScene));
            op.allowSceneActivation = false;

            float timer = 0.0f;
            while (!op.isDone)
            {

                yield return null;

                timer += Time.deltaTime;
                if (op.progress < 0.9f)
                {
                    progressBarFillAmount = Mathf.Lerp(progressBarFillAmount, op.progress, timer);

                    if (progressBarFillAmount >= op.progress)
                    {
                        timer = 0f;
                    }
                }
                else
                {
                    progressBarFillAmount = Mathf.Lerp(progressBarFillAmount, 1f, timer);

                    if (progressBarFillAmount == 1.0f)
                    {
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }

        public int ScenesEnumToInt(Scenes scene) => scene switch
        {
            Scenes.Openning => 0,
            Scenes.Main => 1,
            _ => -1
        };

    }
}