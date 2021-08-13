using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float posA;
    [SerializeField] float posB;

    [SerializeField] bool moveX;
    [SerializeField] bool moveY;

    [SerializeField] float speed;

    private bool gotoBigPos;

    private void Start()
    {
        if(posA < posB)
        {
            float temp = posA;
            posA = posB;
            posB = temp;
        }

        gotoBigPos = true;
    }

    private void Update()
    {
        
    }

}
