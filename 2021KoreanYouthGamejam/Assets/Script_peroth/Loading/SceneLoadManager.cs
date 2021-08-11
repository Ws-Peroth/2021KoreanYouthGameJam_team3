using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace peroth
{
    public enum Scenes
    {
        Openning,
        Main,
        Loading
    }

    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        public float progressBarFillAmount;
        public static Scenes nextScene;

        private void Start() => DontDestroyOnLoad(gameObject);

        public void SceneChange(Scenes nextSceneEnum)
        {
            nextScene = nextSceneEnum;
            SceneManager.LoadScene(ScenesEnumToInt(Scenes.Loading));
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

                Debug.Log(progressBarFillAmount);

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
                        yield return new WaitForSeconds(1f);
                        yield break;
                    }
                }
            }
        }

        public int ScenesEnumToInt(Scenes scene) => scene switch
        {
            Scenes.Openning => 0,
            Scenes.Main => 1,
            Scenes.Loading => (int) Scenes.Loading,
            _ => -1
        };

    }
}