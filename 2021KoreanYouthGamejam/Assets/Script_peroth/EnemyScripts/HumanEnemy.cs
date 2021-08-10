using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace peroth
{
    public class HumanEnemy : PlayerDetect
    {
        public Transform target;
        public Rigidbody2D gameObjectRigidbody;
        public SpriteRenderer spriteRenderer;

        public Vector2 positionA;
        public Vector2 positionB;

        public float moveSpeed = 3.0f;
        public float angleRange = 45f;
        public float distance = 5f;
        public float delayTime = 0.1f;

        Vector3 direction;
        Vector3 transformValue;

        float dotValue = 0f;


        private void Start() => StartCoroutine(EnemyMove());

        public IEnumerator EnemyMove()
        {
            Vector2 smallVector;
            Vector2 bigVector;

            // 벡터값 초기화
            if (positionA.x < positionB.x)
            {
                smallVector = positionA;
                bigVector = positionB;
            }
            else
            {
                smallVector = positionB;
                bigVector = positionA;
            }

            yield return new WaitForSeconds(1f);

            #region HumanEnemyMove
            bool gotoBigPosition = true;

            while (true)
            {
                Vector2 velocityValue;

                // 방향 벡터
                if (gotoBigPosition)
                    velocityValue = bigVector - smallVector;
                else
                    velocityValue = smallVector - bigVector;

                // BigPosition에 도달 하였을 때
                if (transform.position.x >= bigVector.x)
                {
                    // 방향 벡터 반전
                    velocityValue = smallVector - bigVector;
                    gotoBigPosition = false;
                    spriteRenderer.flipX = true;

                    // 가속도 초기화
                    gameObjectRigidbody.velocity = Vector2.zero;
                    yield return new WaitForSeconds(delayTime);
                }
                // SmallPosition에 도달 하였을 때
                else if (transform.position.x <= smallVector.x)
                {
                    // 방향 벡터 반전
                    velocityValue = bigVector - smallVector;
                    gotoBigPosition = true;
                    spriteRenderer.flipX = false;

                    // 가속도 초기화
                    gameObjectRigidbody.velocity = Vector2.zero;
                    yield return new WaitForSeconds(delayTime);
                }

                // 연산한 가속도 적용
                gameObjectRigidbody.velocity = velocityValue * 0.5f;
                yield return null;
            }
            #endregion
        }

        void Update()
        {
            #region CalculateDistanceFromPlayer
            transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

            dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
            direction = target.position - transform.position;

            isCollisionNear = PlayerApproachCheck(distance, direction, transformValue, dotValue);

            if (isCollisionNear) PlayerApproachNear();
            #endregion
        }

        public void PlayerApproachNear() => IsDetected();
        

        private void OnDrawGizmos()
        {
            #region DrawDetectRange
            Vector3 transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

            Handles.color = isCollisionNear ? _red : _blue;
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, angleRange / 2, distance);
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, -angleRange / 2, distance);
            #endregion
        }
    }
}