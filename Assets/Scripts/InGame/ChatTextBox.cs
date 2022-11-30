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
            _bounds = _chatText.textBounds;
        }

        private void OnValidate()
        {

            Debug.Log(_bounds);
            //_chatText.ComputeMarginSize();
        }
    }
}
