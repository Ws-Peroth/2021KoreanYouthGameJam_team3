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

        public void NewGameButtonDown()
        {
            Debug.Log("New Game");
            SceneLoadManager.instance.SceneChange(Scenes.Stage1);
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