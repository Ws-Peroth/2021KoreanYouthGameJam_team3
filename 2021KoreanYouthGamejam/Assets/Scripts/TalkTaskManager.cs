using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTaskManager : MonoBehaviour
{
    public NPC TalkTaskNpc;
    public GameObject door;
    public GameObject mark;

    // Update is called once per frame
    void Update()
    {
        if (TalkTaskNpc.didItTalk)
        {
            door.SetActive(false);
            mark.SetActive(false);
        }
    }
}
