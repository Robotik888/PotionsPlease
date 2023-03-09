using UnityEngine;

namespace PotionsPlease.InGame
{
    public class GameHeaderFloatEffect : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] private float _floatSinPosTime;	
        [SerializeField] private float _floatSinPosAmplitude;

        [Header("Rotation")]
        [SerializeField] private float _floatSinRotTime;
        [SerializeField] private float _floatSinRotAmplitude;


        [Header("Scale")]
        [SerializeField] private float _floatSinScaleTime;
        [SerializeField] private float _floatSinScaleAmplitude;

        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            float GetFloatEffectValue(float time, float amplitude) => Mathf.Sin(Time.time * time) * amplitude;

            transform.position = _startPosition + Vector3.up * GetFloatEffectValue(_floatSinPosTime, _floatSinPosAmplitude);
            transform.rotation = Quaternion.Euler(Vector3.forward * GetFloatEffectValue(_floatSinRotTime, _floatSinRotAmplitude));
            transform.localScale = Vector2.one + Vector2.one * GetFloatEffectValue(_floatSinScaleTime, _floatSinScaleAmplitude);
        }
    }
}
