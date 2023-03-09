using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
    public class LevelModel : ScriptableObject
    {
        public int RatingMax => Orders.Count * 3;

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<OrderModel> Orders { get; private set; }

        public bool GetLevelDoneByRating(int rating) => RatingMax - rating <= 2;
    }
}

