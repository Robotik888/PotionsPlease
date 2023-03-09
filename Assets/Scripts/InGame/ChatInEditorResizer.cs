using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PotionsPlease.InGame
{
    [ExecuteAlways]
    public class ChatInEditorResizer : MonoBehaviour
    {
        [SerializeField] private float _bubbleOffset;
        [Space]
        [SerializeField] private RectTransform _chatBubble;
        [SerializeField] private RectTransform _chatProfile;
        [Space]
        [SerializeField] private RectTransform _textBox;
        [SerializeField] private RectTransform _arrow;
        [Space]
        [SerializeField] private TMP_Text _chatTextCalcualtor;
        [SerializeField] private TMP_Text _chatText;
        [SerializeField] private RectTransform _chatTextBoxInner;

        public void Update()
        {
            if (Application.isPlaying)
                return;

            /// Chat bubble & profile image areas resize
            var profileAreaExtendX = _chatProfile.rect.x;
            _chatBubble.anchoredPosition = Vector2.right * profileAreaExtendX;
            _chatBubble.sizeDelta = Vector2.right * profileAreaExtendX;

            /// Chat bubble textBox and arrow offset
            var arrowExtendsX = _arrow.rect.x - _bubbleOffset;
            _arrow.anchoredPosition = Vector2.right * -_bubbleOffset;
            _textBox.anchoredPosition = Vector2.right * arrowExtendsX / 2;
            _textBox.sizeDelta = Vector2.right * arrowExtendsX;


            /// Text update and textbox size so text can fit
            if (_chatText.text != _chatTextCalcualtor.text)
            {
                _chatTextCalcualtor.ForceMeshUpdate();
                _chatText.text = _chatTextCalcualtor.text;
            }

            var chatTextPadding = _chatText.rectTransform.sizeDelta * -1;
            _chatTextCalcualtor.rectTransform.sizeDelta = chatTextPadding * -1;
            Vector2 textBoundsSize = _chatTextCalcualtor.textBounds.size;

            _chatTextBoxInner.sizeDelta = textBoundsSize + chatTextPadding;
            _chatTextBoxInner.anchoredPosition = Vector2.right * (-textBoundsSize.x / 2 - chatTextPadding.x / 2);
        }
    }
}
