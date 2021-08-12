using System.Collections;
using UnityEditor;
using UnityEngine;

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

        private Vector3 direction;

        private float dotValue;
        private Vector3 transformValue;


        private void Start()
        {
            StartCoroutine(EnemyMove());
        }

        private void Update()
        {
            #region CalculateDistanceFromPlayer

            transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

            dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
            direction = target.position - transform.position;

            isCollisionNear = PlayerApproachCheck(distance, direction, transformValue, dotValue);

            if (isCollisionNear) PlayerApproachNear();

            #endregion
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            #region DrawDetectRange

            var transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

            Handles.color = isCollisionNear ? _red : _blue;
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, angleRange / 2, distance);
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, -angleRange / 2, distance);

            #endregion
        }
        #endif

        public IEnumerator EnemyMove()
        {
            Vector2 smallVector;
            Vector2 bigVector;

            // ���Ͱ� �ʱ�ȭ
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

            var gotoBigPosition = true;

            while (true)
            {
                Vector2 velocityValue;

                // ���� ����
                if (gotoBigPosition)
                    velocityValue = bigVector - smallVector;
                else
                    velocityValue = smallVector - bigVector;

                // BigPosition�� ���� �Ͽ��� ��
                if (transform.position.x >= bigVector.x)
                {
                    // ���� ���� ����
                    velocityValue = smallVector - bigVector;
                    gotoBigPosition = false;
                    spriteRenderer.flipX = true;

                    // ���ӵ� �ʱ�ȭ
                    gameObjectRigidbody.velocity = Vector2.zero;
                    yield return new WaitForSeconds(delayTime);
                }
                // SmallPosition�� ���� �Ͽ��� ��
                else if (transform.position.x <= smallVector.x)
                {
                    // ���� ���� ����
                    velocityValue = bigVector - smallVector;
                    gotoBigPosition = true;
                    spriteRenderer.flipX = false;

                    // ���ӵ� �ʱ�ȭ
                    gameObjectRigidbody.velocity = Vector2.zero;
                    yield return new WaitForSeconds(delayTime);
                }

                // ������ ���ӵ� ����
                gameObjectRigidbody.velocity = velocityValue * 0.5f;
                yield return null;
            }

            #endregion
        }

        public void PlayerApproachNear()
        {
            IsDetected(target.GetComponent<Player>());
        }
    }
}