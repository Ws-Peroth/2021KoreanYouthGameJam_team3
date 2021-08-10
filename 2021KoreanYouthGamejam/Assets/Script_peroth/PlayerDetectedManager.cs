using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class PlayerDetectedManager : Singleton<PlayerDetectedManager>
    {
        public void PlayerDetected(Player player)
        {
            if (player.isCloaking) return;

            Debug.Log("GAME OVER");
        }
    }
}