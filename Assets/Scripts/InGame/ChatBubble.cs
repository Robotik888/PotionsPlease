using Cysharp.Threading.Tasks;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

namespace PotionsPlease.InGame
{
    [ExecuteAlways]
    public class ChatBubble : MonoBehaviour
    {
        [SerializeField] private RectTransform _textBox;
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private float _rightOffset;

        [Space]
        [SerializeField] private TMP_Text _chatTextCalcualtor;
        [SerializeField] private TMP_Text _chatText;
        [SerializeField] private RectTransform _chatTextBoxInner;

        public async UniTask ShowMessageAnim(string message)
        {
            _chatTextCalcualtor.text = message;
            _chatText.text = message;

            UpdateTextBox();

            await UniTask.Delay(1000);
        }

        public void UpdateTextBox()
        {
            var arrowExtendsX = _arrow.rect.x - _rightOffset;

            _arrow.anchoredPosition = Vector2.right * -_rightOffset;
            _textBox.anchoredPosition = Vector2.right * arrowExtendsX / 2;
            _textBox.sizeDelta = Vector2.right * arrowExtendsX;

            //TODO: Find out which of thease are needed (maybe none?)
            _chatTextCalcualtor.SetAllDirty();
            _chatTextCalcualtor.RecalculateClipping();
            _chatTextCalcualtor.RecalculateMasking();

            ///ENDPOINT: Find other update and setdirty methods for proper recalculating textBounds size.
            /// Also the area sizes needs to be updated more properl
            /// next: popup animation (using scale) 

            Vector2 textBoundsSize = _chatTextCalcualtor.textBounds.size;
            var padding = 20;
            _chatTextBoxInner.sizeDelta = textBoundsSize + Vector2.one * padding * 2;
            _chatTextBoxInner.anchoredPosition = Vector2.right * (-textBoundsSize.x / 2);

            _chatText.text = _chatTextCalcualtor.text;
        }

        private void OnValidate()
        {


            _chatText.text = _chatTextCalcualtor.text;

            UpdateTextBox();
        }
    }
}
