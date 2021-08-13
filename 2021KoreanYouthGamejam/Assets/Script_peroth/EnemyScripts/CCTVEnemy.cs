using System.Collections;
using UnityEditor;
using UnityEngine;

namespace peroth
{
    public class CCTVEnemy : PlayerDetect
    {
        public ViewFieldTest fieldOfView;

        public Transform target;
        public SpriteRenderer spriteRenderer;

        public float delayTime = 0.1f;
        public float rotationA;
        public float rotationB;

        public float rotationSpeed = 10f;
        public float angleRange = 45f;
        public float distance = 7f;

        public bool isNeutralized;

        private Vector3 direction;

        private float dotValue;
        private Vector3 transformValue;

        private void Start()
        {
            fieldOfView.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90 + angleRange / 2));
            fieldOfView.fov = angleRange;
            fieldOfView.viewDistance = distance;
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
        private void ShowViewField()
        {

            var transformValue = spriteRenderer.flipX ? transform.right * -1 : transform.right;

            Handles.color = isCollisionNear ? _red : _blue;
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, angleRange / 2, distance);
            Handles.DrawSolidArc(transform.position, Vector3.forward, transformValue, -angleRange / 2, distance);

        }


        public IEnumerator EnemyMove()
        {
            float smallVector;
            float bigVector;

            // ���Ͱ� �ʱ�ȭ
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

            var gotoBigPosition = true;

            var rotationValue = Vector3.forward;

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

                if (transform.rotation.eulerAngles.z <= smallVector)
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

        public void PlayerApproachNear()
        {
            if (isNeutralized) return;
            IsDetected(target.GetComponent<Player>());
        }

        public IEnumerator Neutralize()
        {
            isNeutralized = true;
            yield return new WaitForSeconds(3f);
            isNeutralized = false;
        }


    }
}