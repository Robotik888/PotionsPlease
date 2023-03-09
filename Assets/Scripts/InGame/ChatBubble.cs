using Cysharp.Threading.Tasks;
using PotionsPlease.Util.Systems;
using TMPro;
using UnityEngine;

namespace PotionsPlease.InGame
{
    [ExecuteAlways]
    public class ChatBubble : MonoBehaviour
    {
        [Header("Show & hide animation")]
        [SerializeField] private float _messageBeforeClickDelay;
        [Space]
        [SerializeField] private AnimationCurve _showAnimCurve;
        [SerializeField] private float _showAnimTime;
        [Space]
        [SerializeField] private float _hideAnimTime;
        [SerializeField] private AnimationCurve _hideAnimCurve;

        [Header("Components")]
        [SerializeField] private TMP_Text _chatTextCalcualtor;
        [SerializeField] private TMP_Text _chatText;
        [SerializeField] private RectTransform _chatTextBoxInner;

        private float _chatTextPadding;
        private UniTaskCompletionSource _clickOnChatTcs;

        private void Start()
        {
            _chatTextPadding = _chatText.rectTransform.sizeDelta.x / -2;
            
            UpdateTextBox(string.Empty);
            transform.localScale = Vector3.zero;
        }

        public async UniTask ShowMessageUnclickProcedureAsync(string message, bool hideMessageAfter = true)
        {
            UpdateTextBox(message);

            ///Show
            await ShowMessageAnimAsync(true);

            ///Tiny delay
            await UniTask.Delay(System.TimeSpan.FromSeconds(_messageBeforeClickDelay));

            ///Wait for click
            await ClickOnChatActionAsync();

            ///Hide
            if (hideMessageAfter)
                await ShowMessageAnimAsync(false);
        }

        public void SetMessage(string message)
        {
            UpdateTextBox(message);
        }

        public async UniTask ShowMessageAnimAsync(bool value)
        {
            if (value)
            {
                ///Show
                AudioSystem.PlaySound("Message", 0.1f);
                await transform.LeanScale(Vector3.one, _showAnimTime).setEase(_showAnimCurve).ToUniTaskAsync();
            }
            else
            {
                ///Hide
                await transform.LeanScale(Vector3.zero, _hideAnimTime).setEase(_hideAnimCurve).ToUniTaskAsync();
            }
        }


        public void UpdateTextBox(string message)
        {
            _chatTextCalcualtor.text = message;
            _chatText.text = message;

            _chatTextCalcualtor.ForceMeshUpdate();

            var textBoxSize = (Vector2)_chatTextCalcualtor.textBounds.size + _chatTextPadding * 2 * Vector2.one;
            _chatTextBoxInner.sizeDelta = textBoxSize;
            _chatTextBoxInner.anchoredPosition = Vector2.right * (textBoxSize.x / -2);
        }

        public void NextMessage()
        {
            if (_clickOnChatTcs != null)
                _clickOnChatTcs.TrySetResult();
        }

        private async UniTask ClickOnChatActionAsync()
        {
            _clickOnChatTcs = new UniTaskCompletionSource();
            UIManager.Instance.InGameDarken.OnClick.AddListener(NextMessage);

            await _clickOnChatTcs.Task;

            UIManager.Instance.InGameDarken.OnClick.RemoveListener(NextMessage);
            _clickOnChatTcs = null;
        }

        public void MenuReset()
        {
            _clickOnChatTcs?.TrySetResult();
        }
    }
}
