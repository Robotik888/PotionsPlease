using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class LerpTest : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPos;

        public AnimationCurve curve;
        public float time;
        public float lerpTime;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                transform.position = _startPosition;
            }

            transform.position = Vector3.Lerp(transform.position, _endPos, lerpTime * Time.deltaTime);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            MyDebug.DrawCross(_startPosition, 0.2f);
            MyDebug.DrawCross(_endPos, 0.2f);
        }
    }
}
