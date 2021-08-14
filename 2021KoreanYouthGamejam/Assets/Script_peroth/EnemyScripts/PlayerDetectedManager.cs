using UnityEngine;

namespace peroth
{
    public class PlayerDetectedManager : Singleton<PlayerDetectedManager>
    {
        public GameObject gameOverCanvas;
        private void Start()
        {
            gameOverCanvas.SetActive(false);

        }

        public void PlayerDetected(Player player)
        {
            gameOverCanvas.SetActive(true);
            player.isDetected = true;
            Debug.Log("GAME OVER");
        }
    }
}