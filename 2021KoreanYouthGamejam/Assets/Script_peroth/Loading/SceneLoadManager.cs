using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace peroth
{
    public enum Scenes
    {
        Openning,
        Loading,
        Home1,
        Street1,
        Fctory,
        Street2,
        Stage1,
        Stage2,
        FOH1,
        Stage3,
        Stage4,
        FOH2,
        Cre
    }

    public class SceneLoadManager : SingletonDontDestroy<SceneLoadManager>
    {
        public static Scenes nextScene;
        public float progressBarFillAmount;

        public void SceneChange(Scenes nextSceneEnum)
        {
            nextScene = nextSceneEnum;

            if (nextScene == Scenes.Stage1)
            {
                InventoryManager.instance.AddItem(ItemCode.ItemA);  // 광학?
                InventoryManager.instance.AddItem(ItemCode.ItemB);  // 투명?
            }
            if(nextScene == Scenes.Stage3)
            {
                InventoryManager.instance.AddItem(ItemCode.ItemC);  // 편지?
            }

            StageManager.instance.currentStage = ScenesEnumToInt(nextScene);
            SceneManager.LoadScene(ScenesEnumToInt(Scenes.Loading));

            Debug.Log("Loading Scene Load Finish");
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
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

                        #region Song
                        if (nextScene == Scenes.Openning)
                        {
                            MusicClass.instance.TitleSongOn();
                        }
                        else
                        {
                            MusicClass.instance.StageSongOn();
                        }
                        #endregion

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
                Scenes.Loading  => 1,
                Scenes.Home1    => 2,
                Scenes.Street1  => 3,
                Scenes.Fctory   => 4,
                Scenes.Street2  => 5,
                Scenes.Stage1   => 6,
                Scenes.Stage2   => 7,
                Scenes.FOH1     => 8,
                Scenes.Stage3   => 9,
                Scenes.Stage4   => 10,
                Scenes.FOH2     => 11,
                Scenes.Cre      => 12,
                _ => -1
            };
        }

        public static Scenes IntToScenesEnum(int scene)
        {
            return scene switch
            {
                0 => Scenes.Openning,
                1 => Scenes.Loading,
                2 => Scenes.Home1,
                3 => Scenes.Street1,
                4 => Scenes.Fctory,
                5 =>Scenes.Street2,
                6 => Scenes.Stage1,
                7 => Scenes.Stage2,
                8 => Scenes.FOH1,
                9 => Scenes.Stage3,
                10 => Scenes.Stage4,
                11 => Scenes.FOH2,
                12 => Scenes.Cre,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}