using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/Potion")]
    public class PotionModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ItemModel[] Recipe { get; private set; }
    }
}
