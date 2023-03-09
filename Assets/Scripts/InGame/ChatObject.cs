using Cysharp.Threading.Tasks;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using PotionsPlease.Util.Systems;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class ChatObject : MonoBehaviour, IPointerClickHandler
    {
        [Header("Show & Hide profile anim")]
        [SerializeField] private float _showAnimDuration;
        [SerializeField] private AnimationCurve _showAnimCurve;
        [Space]
        [SerializeField] private float _timeBetweenMessages;
        [Space]
        [SerializeField] private ChatBubble _chatBubble;
        [Space]
        [SerializeField] private RectTransform _chatProfileArea;
        [SerializeField] private RectTransform _chatBubbleArea;
        [SerializeField] private Image _customerProfileImage;

        private CustomerModel _customer;

        private float _profileShowPositionX;
        private float _profileHidePositionX;

        private void Start()
        {
            _profileShowPositionX = _chatProfileArea.anchoredPosition.x;
            _profileHidePositionX = _chatProfileArea.rect.width;
            _chatProfileArea.anchoredPosition = new Vector2(_profileHidePositionX, 0);
        }

        public async UniTask ShowMessageProcedureAsync(params string[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                await _chatBubble.ShowMessageUnclickProcedureAsync(/*$"<b>{_customer.Name}:</b> " + */messages[i]);
                await UniTask.Delay(System.TimeSpan.FromSeconds(_timeBetweenMessages));
            }
        }

        public async UniTask ShowMessagePersistentAsync(string messages)
        {
            //_chatBubble.SetMessage(/*$"<b>{_customer.Name}:</b> " + */messages);
            //await _chatBubble.ShowMessageAnimAsync(true);
            await _chatBubble.ShowMessageUnclickProcedureAsync(messages, false);
        }

        public async UniTask HideMessagePersistentAsync()
        {
            await _chatBubble.ShowMessageAnimAsync(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _chatBubble.NextMessage();
        }

        public void SetCustomer(CustomerModel customer)
        {
            _customerProfileImage.sprite = customer.Sprite;
            _customer = customer;
        }

        public async UniTask ShowProfileAnimAsync(bool value)
        {
            if (value)
                AudioSystem.PlaySound("OrderNew");

            var from = value ? _profileHidePositionX : _profileShowPositionX;
            var to   = value ? _profileShowPositionX : _profileHidePositionX;

            await LeanTween.value(_chatProfileArea.gameObject, from, to, _showAnimDuration)
                .setEase(_showAnimCurve)
                .setOnUpdate((float val) => _chatProfileArea.anchoredPosition = new Vector2(val, 0))
                .ToUniTaskAsync();
        }

        public void MenuReset()
        {
            _chatBubble.MenuReset();
            _ = ShowProfileAnimAsync(false);
            _ = _chatBubble.ShowMessageAnimAsync(false);
        }
    }
}
