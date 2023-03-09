using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/Order")]
    public class OrderModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public CustomerModel Customer { get; private set; }

        [field: Space]
        [field: SerializeField] public string PotionName { get; private set; }
        [field: SerializeField] public ItemModel[] PotionIngredients { get; private set; }
        [field: SerializeField] public ItemModel[] BannedIngredients { get; private set; }

        [field: Space]
        [field: SerializeField] public string[] MessagesOrder { get; private set; }
        [field: SerializeField] public string[] MessagesGoodResponse { get; private set; }
        [field: SerializeField] public string[] MessagesMehResponse { get; private set; }
        [field: SerializeField] public string[] MessagesBadResponse { get; private set; }
    }
}
