using TMPro;
using UnityEngine;

namespace PotionsPlease.InGame
{
    [ExecuteAlways]
    public class ChatTextBox : MonoBehaviour
    {
        [SerializeField] private TMP_Text _chatText;
        [SerializeField] private Bounds _bounds;
        [SerializeField] private RectTransform _rectTransform;

        private void Update()
        {
            //_rectTransform.sizeDelta = _chatText.bounds.extents;//.textBounds.extents;
            _rectTransform.sizeDelta = _chatText.GetRenderedValues(true);
            _rectTransform.sizeDelta = _chatText.GetPreferredValues(true);

            _bounds = _chatText.textBounds;

        }

        private void OnValidate()
        {

            Debug.Log(_bounds);
            //_chatText.ComputeMarginSize();
        }
    }
}
