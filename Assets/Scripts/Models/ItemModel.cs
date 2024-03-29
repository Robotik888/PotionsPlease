using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
    public class ItemModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }
        [field: SerializeField, ] public Sprite Sprite { get; private set; }

        [field: SerializeField, ] public bool IsReverted { get; set; }

    }
}
