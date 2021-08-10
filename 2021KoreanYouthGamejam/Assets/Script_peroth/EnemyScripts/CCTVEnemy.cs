using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace peroth
{
    public class CCTVEnemy : PlayerDetect
    {
        public Transform target;
        public Rigidbody2D gameObjectRigidbody;
        public SpriteRenderer spriteRenderer;

        public float delayTime = 0.1f;
        public float rotationA;
        public float rotationB;

        public float rotationSpeed = 10f;
        public float angleRange = 45f;
        public float distance = 7f;

        Vector3 direction;
        Vector3 transformValue;

        float dotValue = 0f;

        private void Start() => StartCoroutine(EnemyMove());

        public IEnumerator EnemyMove()
        {
            float smallVector;
            float bigVector;

            // 벡터값 초기화
            if (rotationA < rotationB)
            {
                smallVector = rotationA;
                bigVector = rotationB;
            }
            else
            {
                smallVector = rotationB;
                bigVector = rotationA;
            }

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, smallVector));
            yield return new WaitForSeconds(1f);

            #region HumanEnemyMove
            bool gotoBigPosition = true;

            Vector3 rotationValue = Vector3.forward;

            while (true)
            {

                if (gotoBigPosition)
                    rotationValue = Vector3.forward;
                else
                    rotationValue = Vector3.back;

                if (transform.rotation.eulerAngles.z >= bigVector)
                {
                    gotoBigPosition = false;
                    rotationValue = Vector3.zero;
                    transform.Rotate(rotationValue);
                    yield return new WaitForSeconds(delayTime);
                    rotationValue = Vector3.back;
                }
                if(transform.rotation.eulerAngles.z <= smallVector)
                {
                    gotoBigPosition = true;
                    rotationValue = Vector3.zero;
                    transform.Rotate(rotationValue);
                    yield return new WaitForSeconds(delayTime);
                    rotationValue = Vector3.forward;
                }

                transform.Rotate(rotationValue * Time.deltaTime * rotationSpeed);

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

        public void PlayerApproachNear()
        {
            Debug.Log("확신 단계");
        }

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