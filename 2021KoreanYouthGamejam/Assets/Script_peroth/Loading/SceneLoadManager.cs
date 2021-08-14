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

    public class SceneLoadManager : SingletonDontDestroy<SceneLoadManager>
    {
        public static Scenes nextScene;
        public float progressBarFillAmount;

        public void SceneChange(Scenes nextSceneEnum)
        {
            nextScene = nextSceneEnum;
            SceneManager.LoadScene(ScenesEnumToInt(Scenes.Loading));

            Debug.Log("Loading Scene Load Finish");
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            StageManager.instance.currentStage = ScenesEnumToInt(nextScene);
            yield return new WaitForSeconds(1f);

            var op = SceneManager.LoadSceneAsync(ScenesEnumToInt(nextScene));
            op.allowSceneActivation = false;

            var timer = 0.0f;
            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;
                if (op.progress < 0.9f)
                {
                    progressBarFillAmount = Mathf.Lerp(progressBarFillAmount, op.progress, timer);

                    if (progressBarFillAmount >= op.progress) timer = 0f;
                }
                else
                {
                    progressBarFillAmount = Mathf.Lerp(progressBarFillAmount, 1f, timer);

                    if (progressBarFillAmount == 1.0f)
                    {
                        yield return new WaitForSeconds(1f);
                        progressBarFillAmount = 0;
                        op.allowSceneActivation = true;
                        yield break;
                    }
                }
            }
        }

        public static int ScenesEnumToInt(Scenes scene)
        {
            return scene switch
            {
                Scenes.Openning => 0,
                Scenes.Main => 1,
                Scenes.Loading => (int)Scenes.Loading,
                _ => -1
            };
        }

        public static Scenes IntToScenesEnum(int scene)
        {
            return scene switch
            {
                0 => Scenes.Openning,
                1 => Scenes.Main,
                2 => Scenes.Loading,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}