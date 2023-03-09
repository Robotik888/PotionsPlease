using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class BackgroundDarken : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick { get; set; } = new UnityEvent();

        [SerializeField] private float _darkenAlpha;

        [Header("Animations")]
        [SerializeField] private float _fadeAnimDuration;

        [field: Header("Components")]
        [field: SerializeField] public Image BackgroundImage { get; private set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }

        public async UniTask SetDarkenAnimAsync(bool value)
        {
            float from  = value ? 0 : _darkenAlpha;
            float to    = value ? _darkenAlpha : 0;
            
            await LeanTween.value(gameObject, from, to, _fadeAnimDuration)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) => BackgroundImage.color = BackgroundImage.color.SetA(val))
                .ToUniTaskAsync();
        }
    }
}
