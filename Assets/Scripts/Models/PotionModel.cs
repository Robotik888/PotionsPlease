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
        private class ItemPrisada
        {
            public ItemModel ItemModel => _itemModel;
            public float Effectivity => _effectivity;

            [SerializeField] private ItemModel _itemModel;
            [SerializeField, Range(0, 1)] private float _effectivity;

        }

        [SerializeField] private string _name;
        [SerializeField] private ItemPrisada[] _recipe;

        private float _effectivity;

        public void AddItemToRecipe(ItemModel item)
        {
            float currentItemEffectivity = 0;

            for (int i = 0; i < _recipe.Length; i++)
            {
                if (item.name == _recipe[i].ItemModel.Name)
                {
                    currentItemEffectivity = _recipe[i].Effectivity;
                }
            }

            ///vypocet efektivity
        }

        public List<ItemModel> getNeeded()
        {
            List<ItemModel> items = new List<ItemModel>();
            foreach (ItemPrisada ingredient in _recipe)
            {
                items.Add(ingredient.ItemModel);
            }
            return items;
        }
    }
}
