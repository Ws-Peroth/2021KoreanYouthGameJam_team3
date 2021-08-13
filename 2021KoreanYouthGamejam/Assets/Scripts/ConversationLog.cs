using System;
using System.Collections;
using System.Collections.Generic;
using peroth;
using UnityEngine;
using UnityEngine.UI;

public class ConversationLog : MonoBehaviour
{
    private List<string> conversationLog = new List<string>();

    string textLog = "";    // text에 적용될 내용
    string addText = "";    // 추가하는 내용

    public void AddDialogue(string name, string dialogue)
    {
        if (conversationLog.Count > 20)
        {
            conversationLog.Remove(conversationLog[0]);
        }
        conversationLog.Add($"{name}: {dialogue}");

        // logText.text = "";
        // string txt = "";
        textLog = "";
        addText = "";

        foreach (string line in conversationLog)
        {
            addText = addText == "" ? line : $"\n{line}";
        }

        textLog = addText;

        ChatLogManager.instance.chatLog += textLog;
    }
}
