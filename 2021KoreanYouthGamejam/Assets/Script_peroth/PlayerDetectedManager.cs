using UnityEngine;

namespace peroth
{
    public class PlayerDetectedManager : Singleton<PlayerDetectedManager>
    {
        public void PlayerDetected(Player player)
        {
            if (player.isUsingItem) return;

            player.isDetected = true;
            Debug.Log("GAME OVER");
        }
    }
}