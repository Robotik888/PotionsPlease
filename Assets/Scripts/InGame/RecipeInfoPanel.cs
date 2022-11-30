using PotionsPlease.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class RecipeInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _recipeHeaderText;
        [SerializeField] private Image[] _ingredientImages;


        private Dictionary<ItemModel, Image> _recipeImageDict = new Dictionary<ItemModel, Image>();

        public void SetPotion(PotionModel potionModel)
        {
            var recipe = potionModel.GetRecipeItemModels();
            _recipeHeaderText.text = potionModel.Name;
            _recipeImageDict.Clear();

            for (int i = 0; i < _ingredientImages.Length; i++)
            {
                var curentImage = _ingredientImages[i];

                if (i < recipe.Length)
                {
                    curentImage.gameObject.SetActive(true);

                    _recipeImageDict.Add(recipe[i], curentImage);
                    curentImage.sprite = recipe[i].Sprite;
                    curentImage.color = new(0.3f, 0.3f, 0.3f);
                }
                else
                {
                    curentImage.gameObject.SetActive(false);
                }
            }
        }

        public void OnItemAddedToCauldron(ItemModel itemModel)
        {
            if (_recipeImageDict.TryGetValue(itemModel, out Image ingredientImage))
            {
                ingredientImage.color = Color.white;
                _recipeImageDict.Remove(itemModel);
            }
        }
    }
}
