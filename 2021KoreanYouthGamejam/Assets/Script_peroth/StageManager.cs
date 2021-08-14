using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class StageManager : SingletonDontDestroy<StageManager>
    {
        public int currentStage;

        private void Start()
        {
            currentStage = SceneLoadManager.ScenesEnumToInt(Scenes.Openning);
        }

        public void StageClear()
        {
            currentStage++;
            SceneLoadManager.instance.SceneChange(SceneLoadManager.IntToScenesEnum(currentStage));
        }

    }
}