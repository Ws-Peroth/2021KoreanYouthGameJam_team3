using System;
using UnityEngine;

namespace peroth
{
    public class MenuTabManager : Singleton<MenuTabManager>
    {
        [SerializeField] private GameObject optionCanvas;
        [SerializeField] private GameObject keySettingCanvas;
        [SerializeField] private GameObject dialogueLogCanvas;
        [SerializeField] private GameObject menuTab;
        [HideInInspector] public bool isMenuOn;


        public bool isPopup;

        private void Update()
        {
            if (!isPopup)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (!menuTab.activeSelf)
                        ThisTabOpen();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (menuTab.activeSelf)
                        CloseButtonDown();
                }
            }
        }

        public void ThisTabOpen()
        {
            isMenuOn = true;
            Time.timeScale = 0;

            menuTab.SetActive(true);
            InventoryManager.instance.CreatIcon();
            Debug.Log("TabOpen");
        }

        public void CloseButtonDown()
        {
            if (menuTab.activeSelf)
            {
                isMenuOn = false;
                Time.timeScale = 1;
                Debug.Log("Close ManuTab");
                menuTab.SetActive(false);
            }
        }

        public void OptionButtonDown()
        {
            isPopup = true;
            optionCanvas.SetActive(true);
        }

        public void KeySettingButtonDown()
        {
            isPopup = true;
            keySettingCanvas.SetActive(true);
        }

        public void DialogueLogButtonDown()
        {
            isPopup = true;
            ChatLogManager.instance.AddContentToText();
            dialogueLogCanvas.SetActive(true);
            ChatLogManager.instance.ShowText();
        }

        public void SaveAndQuitButtonDown()
        {
            // TODO : SAVE 기능 구현
            // Application.Quit();
            Time.timeScale = 1;
            SceneLoadManager.instance.SceneChange(Scenes.Openning);
        }
    }
}