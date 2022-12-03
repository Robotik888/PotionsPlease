using Cysharp.Threading.Tasks;
using System;
using UnityEditor.VersionControl;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class ChatObject : MonoBehaviour
    {
        [SerializeField] private string[] _messagesTEST;
        [Space]
        [SerializeField] private ChatBubble _chatBubble;
        [Space]
        [SerializeField] private RectTransform _chatProfileArea;
        [SerializeField] private RectTransform _chatBubbleArea;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                OnValidate();
                _ = ShowMessageProcedure(_messagesTEST);    
            }
        }

        private void OnValidate()
        {
            var profileAreaExtendX = _chatProfileArea.rect.x;

            _chatBubbleArea.anchoredPosition = Vector2.right * profileAreaExtendX;
            _chatBubbleArea.sizeDelta = Vector2.right * profileAreaExtendX;
        }

        public async UniTask ShowMessageProcedure(params string[] messages)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                await _chatBubble.ShowMessageAnim(messages[i]);
                //await UniTask.Delay(1000);
            }

        }
    }
}
