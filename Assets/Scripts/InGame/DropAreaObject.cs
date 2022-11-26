using PotionsPlease.Util.Managers;
using Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PotionsPlease.InGame
{
    //[ExecuteAlways]
    public class DropAreaObject : MonoBehaviour
    {
        public bool IsItemHover { get; private set; }

        [Space]
        [SerializeField] private float _borderScrollSpeed;
        [SerializeField] private float _showLerpSpeed;

        [Space]
        [SerializeField] private Vector2 _areaSize;
        [SerializeField] private float _textOffsetY;

        [Header("Alphas")]
        [SerializeField] private float _dragAlpha       = 0.5f;
        [SerializeField] private float _dragHoverAlpha  = 0.8f;

        [Header("Components")]
        [SerializeField] private Rectangle _innerRectShape;
        [SerializeField] private Rectangle _borderRectShape;
        [SerializeField] private ShapeGroup _shapeGroup;
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _targetAlpha;
        private float _alpha;

        private void Start()
        {
            UpdateAlphas();
        }

        private void Update()
        {
            CheckItemDrag();

            _borderRectShape.DashOffset = Time.time * _borderScrollSpeed; /// Border line animation
            
            if (_alpha != _targetAlpha)
            {
                _alpha = Mathf.Lerp(_alpha, _targetAlpha, _showLerpSpeed * Time.deltaTime);

                UpdateAlphas();
            }
        }

        private void CheckItemDrag()
        {
            var draggedItem = GameManager.Instance.DraggedItemCurrent;

            if (draggedItem != null)
            {
                IsItemHover = _innerRectShape.GetWorldBounds().Contains(draggedItem.transform.position);

                _targetAlpha = IsItemHover ? _dragHoverAlpha : _dragAlpha;
            }
            else
            {
                _targetAlpha = 0;
                IsItemHover = false;
            }
        }

        private void OnValidate()
        {
            _innerRectShape.Width   = _areaSize.x;
            _innerRectShape.Height  = _areaSize.y;
            _borderRectShape.Width  = _areaSize.x + _borderRectShape.Thickness;
            _borderRectShape.Height = _areaSize.y + _borderRectShape.Thickness;

            _canvas.position = Vector3.up * (transform.position.y + _borderRectShape.Height / 2 + _textOffsetY);
        }

        private void UpdateAlphas()
        {
            _shapeGroup.Color = _shapeGroup.Color.SetA(_alpha);
            _canvasGroup.alpha = _alpha;
        }
    }
}
