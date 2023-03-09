using Cysharp.Threading.Tasks;
using PotionsPlease.Util.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class PotionOverlay : MonoBehaviour
    {
        public Transform PotionInHandPoint => _potionInHandPoint;

        [SerializeField] private float _handRadius;

        [Header("Animations")]
        [SerializeField] private float _fadeShowPotionTimeDelay;
        [SerializeField] private float _hideAnimDelay;

        [Header("Components")]
        [SerializeField] private PotionObject _potionObject;
        [SerializeField] private HandObject _handObject;
        [SerializeField] private Transform _potionInHandPoint;


        public async UniTask GivePotionProcedureAsync()
        {
            await ShowAnimAsync();
            await DragToHandActionAsync();
            await UniTask.Delay((int)(_hideAnimDelay * 1000));
            await HideAnimAsync();
        }

        public async UniTask ShowAnimAsync()
        {
            var darkenAnimTask = UIManager.Instance.InGameDarken.SetDarkenAnimAsync(true);

            var tcsPotion = new UniTaskCompletionSource();
            LeanTween.delayedCall(gameObject, _fadeShowPotionTimeDelay, async () =>
            {
                await _potionObject.ShowAnimAsync();
                tcsPotion.TrySetResult();
            });

            await UniTask.WhenAll(darkenAnimTask, tcsPotion.Task);
        }

        private async UniTask DragToHandActionAsync()
        {
            _handObject.Show();

            _potionObject.transform.SetParent(transform);
            _potionObject.transform.SetAsLastSibling();

            await _potionObject.DragToHandActionAsync();
        }

        private async UniTask HideAnimAsync()
        {
            await _handObject.HideAnimAsync();
            //await UIManager.Instance.InGameDarken.SetDarkenAnimAsync(false);
        }

        public bool CheckPotionInHand()
        {
            var sqrMag = ((Vector2)_potionInHandPoint.position - (Vector2)_potionObject.transform.position + Vector2.up * _potionObject.DragOffsetY).sqrMagnitude;
            var isInRadius = sqrMag < _handRadius * _handRadius;
            _handObject.SetActive(isInRadius);

            return isInRadius;
        }
    }
}
