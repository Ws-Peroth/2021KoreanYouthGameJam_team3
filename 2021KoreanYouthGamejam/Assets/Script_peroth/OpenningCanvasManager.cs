using UnityEngine;
using UnityEngine.UI;

namespace peroth
{
    public class OpenningCanvasManager : Singleton<OpenningCanvasManager>
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button optionButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private GameObject background;
        [SerializeField] private GameObject optionCanvas;

        private void Start()
        {
            StageManager.instance.currentStage = SceneLoadManager.ScenesEnumToInt(Scenes.Openning);
            MusicClass.instance.TitleSongOn();
        }
        public void NewGameButtonDown()
        {
            Debug.Log("New Game");
            SceneLoadManager.instance.SceneChange(Scenes.Home1);
        }

        public void LoadGameButtonDown()
        {
            Debug.Log("Load Game");
        }

        public void OptionButtonDown()
        {
            optionCanvas.SetActive(true);
        }

        public void CloseButtonDown()
        {
            Application.Quit();
        }
    }
}