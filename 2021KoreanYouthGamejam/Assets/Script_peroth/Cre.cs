using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class Cre : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StageManager.instance.currentStage = SceneLoadManager.ScenesEnumToInt(Scenes.Openning);
                SceneLoadManager.instance.SceneChange(Scenes.Openning);
            }
        }
    }
}