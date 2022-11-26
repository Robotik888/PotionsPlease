using UnityEngine;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;

namespace PotionsPlease.InGame
{
    [SelectionBase]
    public class ItemObject : MonoBehaviour
    {
        private enum ItemObjectState
        {
            Idle,
            Drag,
            Fall
        }

        [SerializeField] private ItemModel _model;

        [Header("Drop to cauldron")]
        [SerializeField] private float _dropBorderY;
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

        private Vector3 _startPos;

        private ItemObjectState _state = ItemObjectState.Idle;

        private void Start()
        {
            _startPos = transform.position;
            _spriteRenderer.sprite = _model.Sprite;

        }

        private void Update()
        {
            switch (_state)
            {
                case ItemObjectState.Idle:
                    DragEffect(_shelfColor, _shelfScale, 0);
                    transform.position = Vector3.Lerp(transform.position, _startPos, _returnLerpTime * Time.deltaTime);
                    break;
                case ItemObjectState.Drag:
                    DragEffect(_dragColor, _dragScale, _dragRotInitial + Mathf.Sin(Time.time * _dragRotSwaySpeed) * _dragRotSwayAmplitude);
                    break;
                case ItemObjectState.Fall:
                    _fallingSpeed = Mathf.Min(_fallingSpeed + _fallingGravity * Time.deltaTime, _fallingMaxSpeed);
                    transform.position -= Vector3.up * _fallingSpeed * Time.deltaTime;
                    transform.rotation *= Quaternion.Euler(0, 0, _fallingSpeed * _fallingRotateFactor * _fallingRotateDir * Time.deltaTime);
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_state == ItemObjectState.Fall && collision.gameObject.CompareTag(Constants.Tags.CAULDRON))
            {
                Destroy(gameObject);
                GameManager.Instance.Cauldron.AddItem(_model);
            }
        }

        private void OnMouseDrag()
        {
            if (_state == ItemObjectState.Fall)
                return;

            _state = ItemObjectState.Drag;
            Vector3 mouseWorldPos = GameManager.CameraMain.ScreenToWorldPoint(Input.mousePosition).SetZ(0);
            transform.position = mouseWorldPos;

            GameManager.Instance.DraggedItemCurrent = this;

            UIManager.Instance.changeTexts(_model.Name, _model.Description);
        }

        private void OnMouseUp()
        {
            if (_state == ItemObjectState.Fall)
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

        private void DragEffect(Color targetColor, float targetScale, float targetRotation)
        {
            transform.localScale = Vector2.one * Mathf.Lerp(transform.localScale.x, targetScale, _dragScaleLerpTime * Time.deltaTime);
            _spriteRenderer.color = targetColor;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotation), _dragRotLerpTime * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(new Vector2(-50, _dropBorderY), new Vector2(50, _dropBorderY));
        }
    }
}
