using Cysharp.Threading.Tasks;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class PotionObject : MonoBehaviour, IPointerDownHandler
    {
        private Vector2 InHandPos => PotionManager.Instance.PotionOverlay.PotionInHandPos + Vector2.up * _rectTransform.sizeDelta.y / 2;

        [SerializeField] private float _idlePosLerpTime;

        [Header("Animations")]
        [SerializeField] private float _showAnimTime;
        [SerializeField] private float _showAnimStartYOffset;
        [SerializeField] private float _handSetAnimTime;

        [Header("Components")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private PotionModel _model;
        [SerializeField] private Image _image;

        private bool _isDraggable;
        private bool _isMouseDrag;
        private bool _inHandPointFollow;


        private Vector2 _idlePos;
        private Vector2 _handSetAnimFromPos;

        private UniTaskCompletionSource _dragToHandTcs;

        private void Start()
        {
            _idlePos = transform.position;
            _image.color = _image.color.SetA(0);

        }

        private void Update()
        {
            if (_inHandPointFollow)
            {
                transform.position = InHandPos;
                return;
            }

            if (_isMouseDrag)
            {
                transform.position = Input.mousePosition;

                var isInHand = PotionManager.Instance.PotionOverlay.CheckPotionInHand();

                var isDropInput = Input.GetMouseButtonUp(0);
                if (isDropInput)
                {
                    if (isInHand)
                    {
                        _isDraggable = false;
                        _handSetAnimFromPos = transform.position;

                        LeanTween.delayedCall(gameObject, _handSetAnimTime * 0.2f, () => PotionManager.Instance.PotionOverlay.SetPotionBehindHand());
                        LeanTween.value(gameObject, 0, 1, _handSetAnimTime)
                            .setOnUpdate((float val) => transform.position = Vector3.Lerp(_handSetAnimFromPos, InHandPos, val))
                            .setEase(LeanTweenType.easeOutQuint)
                            .setOnComplete(() => _dragToHandTcs.TrySetResult());
                    }

                    _isMouseDrag = false;
                }
            }
            else if (_isDraggable)
            {
                transform.position = Vector2.Lerp(transform.position, _idlePos, _idlePosLerpTime * Time.deltaTime);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isDraggable)
                _isMouseDrag = true;
        }

        public async UniTask ShowAnimAsync()
        {
            _inHandPointFollow = false;

            var tcs = new UniTaskCompletionSource();

            LeanTween.value(gameObject, 0, 1, _showAnimTime)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) =>
                {
                    transform.position = Vector2.Lerp(_idlePos + Vector2.up * _showAnimStartYOffset, _idlePos, val);
                    _image.color = _image.color.SetA(val);
                })
                .setOnComplete(() => tcs.TrySetResult());

            await tcs.Task;
        }

        public async UniTask DragToHandActionAsync()
        {
            _isDraggable = true;
            _inHandPointFollow = false;

            _dragToHandTcs = new UniTaskCompletionSource();
            await _dragToHandTcs.Task;

            _inHandPointFollow = true;
        }
    }
}
