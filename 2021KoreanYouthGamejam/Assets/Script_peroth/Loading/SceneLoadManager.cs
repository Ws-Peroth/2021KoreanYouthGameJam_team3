using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace peroth
{
    public enum Scenes
    {
        Openning,
        Loading,
        Stage1,
        Stage2,
        Stage3,
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
                Scenes.Loading => 1,
                Scenes.Stage1 => 2,
                Scenes.Stage2 => 3,
                Scenes.Stage3 => 4,
                _ => -1
            };
        }

        public static Scenes IntToScenesEnum(int scene)
        {
            return scene switch
            {
                0 => Scenes.Openning,
                1 => Scenes.Loading,
                2 => Scenes.Stage1,
                3 => Scenes.Stage2,
                4 => Scenes.Stage3,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}