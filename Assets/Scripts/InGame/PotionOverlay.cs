using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class PotionOverlay : MonoBehaviour
    {
        public Vector2 PotionInHandPos => _potionInHandPoint.position;
        public HandObject HandObject => _handObject;

        [SerializeField] private float _handRadius;

        [Header("Animations")]
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _fadeShowPotionTimeDelay;
        [SerializeField] private float _hideAnimDelay;

        [Header("Components")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private PotionObject _potionObject;
        [SerializeField] private HandObject _handObject;
        [SerializeField] private Transform _potionInHandPoint;

        private float _backgroundAlpha;

        private void Awake()
        {
            _backgroundAlpha = _backgroundImage.color.a;
            _backgroundImage.color = _backgroundImage.color.SetA(0);
        }

        public async UniTask ShowNewPotionProcedure()
        {
            await ShowAnimAsync();
            await DragToHandActionAsync();
            await UniTask.Delay((int)(_hideAnimDelay * 1000));
            await HideAnimAsync();
        }

        public async UniTask ShowAnimAsync()
        {
            /// Background
            var tcsBackground = new UniTaskCompletionSource();
            LeanTween.value(gameObject, 0, _backgroundAlpha, _fadeTime)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) => _backgroundImage.color = _backgroundImage.color.SetA(val))
                .setOnComplete(() => tcsBackground.TrySetResult());

            /// Potion 
            var tcsPotion = new UniTaskCompletionSource();
            LeanTween.delayedCall(gameObject, _fadeShowPotionTimeDelay, async () =>
            {
                await _potionObject.ShowAnimAsync();
                tcsPotion.TrySetResult();
            });

            await UniTask.WhenAll(tcsBackground.Task, tcsPotion.Task);
        }

        private async UniTask DragToHandActionAsync()
        {
            _handObject.Show();

            /// Set potion in front of hand image
            _potionObject.transform.SetParent(transform);
            _potionObject.transform.SetAsLastSibling();

            await _potionObject.DragToHandActionAsync();
        }

        private async UniTask HideAnimAsync()
        {
            await _handObject.HideAnimAsync();

            var tcs = new UniTaskCompletionSource();
            
            LeanTween.value(gameObject, _backgroundAlpha, 0, _fadeTime)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) => _backgroundImage.color = _backgroundImage.color.SetA(val))
                .setOnComplete(() => tcs.TrySetResult());

            await tcs.Task;

            ///Try set direction
        }

        public bool CheckPotionInHand()
        {
            var sqrMag = (PotionInHandPos - (Vector2)_potionObject.transform.position).sqrMagnitude;

            var isInRadius = sqrMag < _handRadius * _handRadius;

            _handObject.SetActive(isInRadius);
            //if (isInRadius)

            return isInRadius;
        }

        public void SetPotionBehindHand()
        {
            /// Set potion behind of hand image (and as anchor parent for animation movement)
            _potionObject.transform.SetParent(_handObject.AnchorRectTransform);
            _potionObject.transform.SetAsFirstSibling();
        }
    }
}
