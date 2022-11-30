using PotionsPlease.Models;
using PotionsPlease.Util;
using TMPro;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class UIManager : ManagerBase<UIManager>
    {
        [SerializeField] public TMP_Text _itemInfoHeaderText;
        [SerializeField] public TMP_Text _itemInfoDescText;

        [field: Space]
        [field: SerializeField] public RecipeInfoPanel RecipeInfoPanel { get; private set; }

        public void SetItemInfoText(ItemModel itemModel)
        {
            _itemInfoHeaderText.text = itemModel.Name;
            _itemInfoDescText.text = itemModel.Description;
        }

    }
}
