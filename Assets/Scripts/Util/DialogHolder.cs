using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Util
{
    public class DialogHolder
    {
        private string orderMessage;
        private string goodResponse;
        private string mehResponse;
        private string badResponse;

        public DialogHolder(string order, string good, string meh, string bad)
        {
            orderMessage = order;
            goodResponse = good;
            mehResponse = meh;
            badResponse = bad;
        }

        public string getOrderMessage()
        {
            return orderMessage;
        }
        public string getGoodResponse()
        {
            return goodResponse;
        }
        public string getMehResponse()
        {
            return mehResponse;
        }
        public string getBadResponse()
        {
            return badResponse;
        }

    }
}
