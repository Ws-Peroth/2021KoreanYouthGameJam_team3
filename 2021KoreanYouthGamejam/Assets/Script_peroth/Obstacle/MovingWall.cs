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
    [SerializeField] float delay;

    private bool gotoBigPos;
    private Vector2 moveDir = Vector2.zero;

    private void Start()
    {
        moveDir = Vector2.zero;

        if (moveX) moveDir.x++;
        if (moveY) moveDir.y++;

        if (posA < posB)
        {
            float temp = posA;
            posA = posB;
            posB = temp;
        }

        gotoBigPos = true;

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (moveX)
            {
                if (transform.localPosition.x > posA)
                {
                    gotoBigPos = false;
                    MoveWall();
                    yield return new WaitForSeconds(delay);
                }
                else if (transform.localPosition.x < posB)
                {
                    gotoBigPos = true;
                    MoveWall();
                    yield return new WaitForSeconds(delay);
                }
            }
            else if(moveY)
            {
                if (transform.localPosition.y > posA)
                {
                    gotoBigPos = false;
                    MoveWall();
                    yield return new WaitForSeconds(delay);
                }
                else if (transform.localPosition.y < posB)
                {
                    gotoBigPos = true;
                    MoveWall();
                    yield return new WaitForSeconds(delay);
                }
            }

            MoveWall();
            yield return null;
        }
    }

    private void MoveWall()
    {
        if (gotoBigPos)
            transform.Translate(moveDir * Time.deltaTime * speed);
        else
            transform.Translate(moveDir * Time.deltaTime * -speed);
    }
}
