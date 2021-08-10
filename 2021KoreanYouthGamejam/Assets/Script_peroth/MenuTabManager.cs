using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class MenuTabManager : Singleton<MenuTabManager>
    {
        [SerializeField] private GameObject optionCanvas;
        [SerializeField] private GameObject keySettingCanvas;
        [SerializeField] private GameObject DialogueLogCanvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                gameObject.SetActive(false);
        }

        public void CloseButtonDown() => gameObject.SetActive(false);
        public void OptionButtonDown() => optionCanvas.SetActive(true);
        public void KeySettingButtonDown() => keySettingCanvas.SetActive(true);
        public void DialogueLogButtonDown() => DialogueLogCanvas.SetActive(true);
        public void SaveAndQuitButtonDown()
        {
            // TODO : SAVE 기능 추가
            Application.Quit();
        }
    }
}