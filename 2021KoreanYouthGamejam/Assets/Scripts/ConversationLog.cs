using System;
using System.Collections;
using System.Collections.Generic;
using peroth;
using UnityEngine;
using UnityEngine.UI;

public class ConversationLog : MonoBehaviour
{
    private List<string> conversationLog = new List<string>();
    [SerializeField] private Text logText;

    public void AddDialogue(string name, string dialogue)
    {
        if (conversationLog.Count > 20)
        {
            conversationLog.Remove(conversationLog[0]);
        }
        conversationLog.Add($"{name}: {dialogue}");
        logText.text = "";
        foreach (string line in conversationLog)
        {
            if (logText.text == "") logText.text += line;
            else logText.text += "\n" + line;
        }
    }
}
