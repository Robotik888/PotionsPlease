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

        public bool GetLevelDoneByRating(int rating, bool levelIndex) { 
            if (levelIndex)
            {
                return rating == 3;
            }
            return RatingMax - rating <= 2; 
        }
    }
}

