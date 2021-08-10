using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace peroth
{
    public class PlayerDetect : MonoBehaviour
    {
        public Color _blue = new Color(0f, 0f, 1f, 0.2f);
        public Color _red = new Color(1f, 0f, 0f, 0.2f);

        public Color _green = new Color(0, 1, 0, 0.2f);
        public Color _cyan = new Color(0, 1, 1, 0.2f);

        public bool isCollisionNear = false;
        public bool isCollisionFar = false;

        public bool PlayerApproachCheck(float distance, Vector3 direction, Vector3 transformValue, float dotValue)
        {
            if (direction.magnitude < distance)
                return Vector3.Dot(direction.normalized, transformValue) > dotValue;
            else return false;
        }
    }
}