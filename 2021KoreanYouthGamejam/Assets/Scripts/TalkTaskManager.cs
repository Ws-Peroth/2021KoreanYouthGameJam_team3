using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTaskManager : MonoBehaviour
{
    public NPC[] TalkTaskNpc;
    public GameObject door;
    public GameObject[] mark;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < TalkTaskNpc.Length; i++)
        {
            if (TalkTaskNpc[i].didItTalk)
            {
                door.SetActive(false);
                mark[i].SetActive(false);
            }
        }
    }
}
