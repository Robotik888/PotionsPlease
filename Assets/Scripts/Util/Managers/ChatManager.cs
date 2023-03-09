using Cysharp.Threading.Tasks;
using PotionsPlease.InGame;
using PotionsPlease.Models;
using System.Linq;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class ChatManager : ManagerBase<ChatManager>
    {
        [SerializeField] private ChatObject _chatObject;

        public async UniTask ShowMessageOrder(string[] messages)
        {
            if (messages.Length != 1)
                await ShowMessageUnclick(messages.SkipLast(1).ToArray());

            await _chatObject.ShowMessagePersistentAsync(messages.Last());
        }

        public async UniTask ShowMessageUnclick(string[] messages)
        {
            await _chatObject.ShowMessageProcedureAsync(messages);
        }

        public async UniTask ShowMessageResponse(OrderModel order, int rating)
        {
            string[] response = GetOrderResponseByRating(order, rating);

            /// Add rating as last message
            response = response.Append($"{rating}/3 stars").ToArray();

            await _chatObject.ShowMessageProcedureAsync(response);
        }

        public string[] GetOrderResponseByRating(OrderModel order, int rating)
        {
            string[] response;

            switch (rating)
            {
                case 0:
                    response = order.MessagesBadResponse;
                    break;
                case 1:
                case 2:
                    response = order.MessagesMehResponse;
                    break;
                case 3:
                    response = order.MessagesGoodResponse;
                    break;
                default:
                    throw new System.ArgumentException($"Rating must be between 0 and 3; current rating: {rating}", nameof(rating));
            }

            return response;
        }

        public async UniTask HideOrderLastMessage() => await _chatObject.HideMessagePersistentAsync();

        public void SetCustomer(CustomerModel customer) => _chatObject.SetCustomer(customer);

        public async UniTask ShowProfileAnimAsync(bool value) => await _chatObject.ShowProfileAnimAsync(value);
    }
}
