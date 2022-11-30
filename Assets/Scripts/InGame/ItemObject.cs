using UnityEngine;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using TMPro;

namespace PotionsPlease.InGame
{
    [SelectionBase]
    public class ItemObject : MonoBehaviour
    {
        private enum ItemObjectState
        {
            Idle,
            Drag,
            Fall,
            InCauldron, /// Invisible, undraggable
            Inactive
        }
        public ItemModel ItemModel { get; private set; }
        private bool IsDraggable => _state != ItemObjectState.Fall && _state != ItemObjectState.InCauldron && _state != ItemObjectState.Inactive;

        [Header("Drop to cauldron")]
        [SerializeField] private float _fallingGravity;
        [SerializeField] private float _fallingMaxSpeed;
        [SerializeField] private float _fallingRotateFactor;
        private float _fallingSpeed;
        private float _fallingRotateDir;

        [Header("Drag from shelf effect")]
        [SerializeField] private Color _shelfColor = Color.white;
        [SerializeField] private Color _dragColor = Color.white;    /// Default
        [SerializeField] private float _shelfScale = 1;             /// Default
        [SerializeField] private float _dragScale = 1;
        [SerializeField] private float _dragRotInitial;
        [SerializeField] private float _dragRotSwayAmplitude;
        [SerializeField] private float _dragRotSwaySpeed = 1;

        [Space]
        [SerializeField] private float _dragScaleLerpTime;
        [SerializeField] private float _dragRotLerpTime;
        [SerializeField] private float _returnLerpTime;

        [Header("Components")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField, HideInInspector] private Vector3 _shelfPos;
        private ItemObjectState _state = ItemObjectState.Idle;

        public void Initialize(Vector2 shelfPos)
        {
            _shelfPos = shelfPos;
            transform.position = _shelfPos;
        }

        private void Update()
        {
            switch (_state)
            {
                case ItemObjectState.Idle:
                    DragEffect(_shelfColor, _shelfScale, 0);
                    transform.position = Vector3.Lerp(transform.position, _shelfPos, _returnLerpTime * Time.deltaTime);
                    break;
                case ItemObjectState.Drag:
                    DragEffect(_dragColor, _dragScale, _dragRotInitial + Mathf.Sin(Time.time * _dragRotSwaySpeed) * _dragRotSwayAmplitude);
                    break;
                case ItemObjectState.Fall:
                    _fallingSpeed = Mathf.Min(_fallingSpeed + _fallingGravity * Time.deltaTime, _fallingMaxSpeed);
                    transform.position -= Vector3.up * _fallingSpeed * Time.deltaTime;
                    transform.rotation *= Quaternion.Euler(0, 0, _fallingSpeed * _fallingRotateFactor * _fallingRotateDir * Time.deltaTime);
                    break;
                case ItemObjectState.InCauldron:
                    break;
                case ItemObjectState.Inactive:
                    DragEffect(_shelfColor, _shelfScale, 0);
                    transform.position = Vector3.Lerp(transform.position, _shelfPos, _returnLerpTime * Time.deltaTime);
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_state == ItemObjectState.Fall && collision.gameObject.CompareTag(Constants.Tags.CAULDRON))
            {
                _spriteRenderer.color = _spriteRenderer.color.SetA(0);
                _state = ItemObjectState.InCauldron;
                GameManager.Instance.Cauldron.AddItem(ItemModel);
            }
        }

        private void OnMouseDrag()
        {
            if (!IsDraggable)
                return;

            _state = ItemObjectState.Drag;
            Vector3 mouseWorldPos = GameManager.CameraMain.ScreenToWorldPoint(Input.mousePosition).SetZ(0);
            transform.position = mouseWorldPos;

            GameManager.Instance.DraggedItemCurrent = this;
        }

        private void OnMouseUp()
        {
            if (!IsDraggable)
                return;

            GameManager.Instance.DraggedItemCurrent = null;

            var isInDropArea = GameManager.Instance.DropAreaObject.IsItemHover;

            if (isInDropArea)
            {
                _state = ItemObjectState.Fall;
                _fallingRotateDir = Random.value > 0.5f ? -1 : 1;
            }
            else
            {
                _state = ItemObjectState.Idle;
            }
        }

        private void OnMouseDown()
        {
            if (!IsDraggable)
                return;

            UIManager.Instance.SetItemInfoText(ItemModel);
        }

        public void ResetState(ItemModel itemModel)
        {
            ItemModel = itemModel;
            _spriteRenderer.sprite = itemModel.Sprite;
            _state = ItemObjectState.Idle;
            transform.SetPositionAndRotation(_shelfPos, Quaternion.identity);
            transform.localScale = Vector3.one * _shelfScale;
            _fallingSpeed = 0;
        }


        public void SetIncactive()
        {
            if (_state == ItemObjectState.InCauldron)
                return;

            _state = ItemObjectState.Inactive;
        }

        private void DragEffect(Color targetColor, float targetScale, float targetRotation)
        {
            transform.localScale = Vector2.one * Mathf.Lerp(transform.localScale.x, targetScale, _dragScaleLerpTime * Time.deltaTime);
            _spriteRenderer.color = targetColor;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation), _dragRotLerpTime * Time.deltaTime);
        }
    }
}
