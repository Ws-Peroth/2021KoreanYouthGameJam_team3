using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace peroth {
    public class ChatLogManager : Singleton<ChatLogManager>
    {
        private Transform textTransform;
        [SerializeField] Transform textContentTransform;
        [SerializeField] GameObject textPrefab;
        private GameObject textObj;

        public string chatLog;
        private Text logText;

        private void Start()
        {
            textObj = Instantiate(textPrefab, transform);
            logText = textObj.GetComponent<Text>();
            textTransform = textObj.transform;

            textObj.SetActive(false);
        }

        public void AddContentToText()
        {
            logText.text = chatLog;
        }

        public void RemoveContentToText()
        {
            logText.text = "";
            if (textTransform.parent != transform)
                textTransform.SetParent(transform);
        }

        public void ShowText()
        {
            textTransform.SetParent(textContentTransform);
            textTransform.localPosition = Vector3.zero;
            textTransform.localScale = Vector2.one;
            textObj.SetActive(true);
        }
    }
}