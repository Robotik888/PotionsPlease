using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
    public class LevelModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<OrderModel> Orders { get; private set; }
    }
}

