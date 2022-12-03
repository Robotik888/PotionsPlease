using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/Order")]
    public class OrderModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public PotionModel Potion { get; private set; }
        [field: SerializeField] public CustomerModel Customer { get; private set; }

        //[field: SerializeField] public PotionsPlease.Util.DialogHolder Dialog { get; private set; }  // MAYBE IN FUTURE TODO

        [field: SerializeField, Multiline] public string OrderLine { get; private set; }
        [field: SerializeField, Multiline] public string GoodResponse { get; private set; }
        [field: SerializeField, Multiline] public string MehResponse { get; private set; }
        [field: SerializeField, Multiline] public string BadResponse { get; private set; }
    }
}
