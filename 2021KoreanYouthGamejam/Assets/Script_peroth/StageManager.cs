using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class StageManager : SingletonDontDestroy<StageManager>
    {
        public int currentStage;

        public void StageClear()
        {
            currentStage++;
            Scenes scene = SceneLoadManager.IntToScenesEnum(currentStage);
            SceneLoadManager.instance.SceneChange(scene);
            Debug.Log(scene);
        }

    }
}