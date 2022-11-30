using Cysharp.Threading.Tasks;
using Shapes;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class HandObject : MonoBehaviour
    {
        public RectTransform AnchorRectTransform => _anchorRectTransform;

        [SerializeField] private float _noiseMoveIntensity;
        [SerializeField] private float _noiseMoveSpeed;

        [Header("ShowAnim")]
        [SerializeField] private float _showLerpTime;

        [Header("Hide anim")]
        [SerializeField] private float _hideAnimTime;
        [SerializeField] private AnimationCurve _hideAnimCurve;

        [Header("Components")]
        [SerializeField] private RectTransform _anchorRectTransform;
        [SerializeField] private RectTransform _handRectTransform;

        private bool _isShown;
        private Vector2 _hidePos;


        private void Start()
        {
            _hidePos = Vector3.right * _anchorRectTransform.sizeDelta.x;
            _anchorRectTransform.anchoredPosition = _hidePos;
        }


        private void Update()
        {
            if (_isShown)
                _anchorRectTransform.anchoredPosition = Vector2.Lerp(_anchorRectTransform.anchoredPosition, Vector2.zero, _showLerpTime * Time.deltaTime);

            var noiseMoveDir = new Vector2(
                Mathf.PerlinNoise(Time.time * _noiseMoveSpeed, 0),          /// 0 to 1 
                Mathf.PerlinNoise(0, Time.time * _noiseMoveSpeed) - 0.5f    /// -1 to 1
            );
            _handRectTransform.anchoredPosition = noiseMoveDir * _noiseMoveIntensity;
        }


        public void Show()
        {
            _isShown = true;
        }

        public void SetActive(bool isInRadius)
        {
            //_showAnimTimeTarget = 1;
        }

        public async UniTask HideAnimAsync()
        {
            _isShown = false;

            var tcs = new UniTaskCompletionSource();

            _anchorRectTransform.LeanMove((Vector3)_hidePos, _hideAnimTime)
                .setEase(_hideAnimCurve)
                .setOnComplete(() => tcs.TrySetResult());

            await tcs.Task;

            _handRectTransform.anchoredPosition = Vector3.zero;
        }
    }
}
