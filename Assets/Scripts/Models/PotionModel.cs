using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionsPlease.Models
{
    [CreateAssetMenu(fileName = "Potion", menuName = "ScriptableObjects/Potion")]
    public class PotionModel : ScriptableObject
    {
        [System.Serializable]
        private class Ingredient
        {
            [field: SerializeField] public ItemModel ItemModel { get; private set; }
            [field: SerializeField, Range(0, 1)] public float Effectivity { get; private set; } 

        }

        public string Name => _name;

        [SerializeField] private string _name;
        [SerializeField] private Ingredient[] _recipe;


        public ItemModel[] GetRecipeItemModels() => _recipe.Select(e => e.ItemModel).ToArray();
    }
}
