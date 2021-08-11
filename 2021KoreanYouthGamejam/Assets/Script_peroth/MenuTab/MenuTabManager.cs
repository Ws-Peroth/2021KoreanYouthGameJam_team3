using UnityEngine;

namespace peroth
{
    public class MenuTabManager : Singleton<MenuTabManager>
    {
        [SerializeField] private GameObject optionCanvas;
        [SerializeField] private GameObject keySettingCanvas;
        [SerializeField] private GameObject dialogueLogCanvas;
        [SerializeField] private GameObject menuTab;


        public bool isPopup;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !isPopup)
            {
                if (menuTab.activeSelf)
                    CloseButtonDown();
                else
                    ThisTabOpen();
            }
        }

        public void ThisTabOpen()
        {
            Time.timeScale = 0;

            menuTab.SetActive(true);
            InventoryManager.instance.CreatIcon();
            Debug.Log("TabOpen");
        }

        public void CloseButtonDown()
        {
            if (menuTab.activeSelf)
            {
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
            dialogueLogCanvas.SetActive(true);
        }

        public void SaveAndQuitButtonDown()
        {
            // TODO : SAVE 기능 구현

            Application.Quit();
        }
    }
}