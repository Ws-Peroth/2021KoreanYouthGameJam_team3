using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class PlayerDetectedManager : Singleton<PlayerDetectedManager>
    {
        public GameObject player;
        public Player playerScript;

        public void PlayerDetected()
        {
            Debug.Log("GAME OVER");

        }
    }
}