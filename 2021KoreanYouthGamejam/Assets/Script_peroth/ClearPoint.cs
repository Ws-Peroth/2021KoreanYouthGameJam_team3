using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class ClearPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("enter");
            StageManager.instance.StageClear();
        }
    }
}