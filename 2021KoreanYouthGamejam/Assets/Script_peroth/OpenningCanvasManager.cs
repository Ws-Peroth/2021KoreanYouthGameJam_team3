using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace peroth {
    public enum CanvasName
    {
        Option,
        Close
    }

    public class OpenningCanvasManager : Singleton<OpenningCanvasManager>
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private GameObject background;
        [SerializeField] private GameObject optionCanvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OptionCloseDown();
        }

        public void NewGameButtonDown()
        {
            Debug.Log("New Game");
        }

        public void LoadGameButtonDown()
        {
             
            Debug.Log("Load Game");
        }

        public void OptionButtonDown() => optionCanvas.SetActive(true);

        public void OptionCloseDown() => optionCanvas.SetActive(false);

        public void CloseButtonDown() => Application.Quit();
    }
}