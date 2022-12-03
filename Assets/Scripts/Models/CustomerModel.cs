using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Customer", menuName = "ScriptableObjects/Customer")]
    public class CustomerModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Multiline] public string Lore { get; private set; }
        [field: SerializeField,] public Sprite Sprite { get; private set; }
    }
}
