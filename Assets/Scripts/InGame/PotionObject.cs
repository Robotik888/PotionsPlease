using Cysharp.Threading.Tasks;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using PotionsPlease.Util.Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class PotionObject : MonoBehaviour, IPointerDownHandler
    {
        [field: SerializeField] public float DragOffsetY { get; private set; }
        [SerializeField] private float _idlePosLerpTime;

        [Header("Animations")]
        [SerializeField] private float _showAnimTime;
        [SerializeField] private float _showAnimStartYOffset;
        [SerializeField] private float _handSetAnimTime;

        [Header("Components")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        private bool _isDraggable;
        private bool _isMouseDrag;


        private Vector2 _idlePos;
        private Vector2 _handSetAnimFromPos;

        private UniTaskCompletionSource _dragToHandTcs;

        private void Start()
        {
            _idlePos = transform.position;
            _image.color = _image.color.SetA(0);

            /// Preserve finger offset for mouse control
            if (SystemInfo.deviceType == DeviceType.Desktop)
                DragOffsetY = 0;
        }

        private void Update()
        {
            if (_isMouseDrag)
            {
                transform.position = Input.mousePosition + Vector3.up * DragOffsetY;

                var isInHand = UIManager.Instance.PotionOverlay.CheckPotionInHand();

                if (Input.GetMouseButtonUp(0))
                {
                    AudioSystem.PlaySound("ItemDragStop");

                    if (isInHand)
                    {
                        _isDraggable = false;
                        _handSetAnimFromPos = transform.position;

                        transform.SetParent(UIManager.Instance.PotionOverlay.PotionInHandPoint);
                        transform.LeanMoveLocal(Vector3.up * _rectTransform.sizeDelta.y / 2, _handSetAnimTime)
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
            {
                _isMouseDrag = true;
                AudioSystem.PlaySound("ItemDrag");
            }
        }

        public async UniTask ShowAnimAsync()
        {
            AudioSystem.PlaySound("CreatePotion");

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

            _dragToHandTcs = new UniTaskCompletionSource();
            await _dragToHandTcs.Task;
        }
    }
}
