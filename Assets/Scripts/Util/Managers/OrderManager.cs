using PotionsPlease.InGame;
using PotionsPlease.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class OrderManager : ManagerBase<OrderManager>
    {
        public PotionModel PotionCurrent { get; private set; }

        /// Temporary solution for holding all item and potion models; probably will be changed later to load from resources
        [SerializeField] private LevelModel _currentLevel;

        [SerializeField] private ItemModel[] _itemsAll;       
        [SerializeField] private PotionModel[] _potionsAll;

        [SerializeField] private int orderIndex = -1;



        [Space]
        [SerializeField] private ShelveObject[] _shelves;

        private void Start()
        {
            NextOrder();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                NextOrder();
        }

        public void NextOrder()
        {
 
            if (orderIndex == _currentLevel.Orders.Count - 1) // Temporary solution
            {
                orderIndex = 0;
            } else
            {
                orderIndex += 1;
            }
            PotionCurrent = _currentLevel.Orders[orderIndex].Potion;
            UIManager.Instance.RecipeInfoPanel.SetPotion(PotionCurrent);

            GenerateItems();
        }

        public void EndOrder(ItemModel[] items)
        {
            PotionManager.Instance.NewPotion(items);

            for (int i = 0; i < _shelves.Length; i++)
                _shelves[i].SetItemsInactive();
        }

        private void GenerateItems()
        {
            var requiredItems = PotionCurrent.GetRecipeItemModels();
            var itemsToGenerate = _shelves.Sum(e => e.Size) - requiredItems.Length;

            var itemPool = _itemsAll.Except(requiredItems).ToList();
            var generatedItems = new List<ItemModel>(requiredItems);

            while (itemsToGenerate != 0)
            {
                var randomItemIndex = Random.Range(0, itemPool.Count);

                generatedItems.Add(itemPool[randomItemIndex]);

                if (itemPool.Count > 1) /// Temporary solution for not running out of items in pool
                    itemPool.RemoveAt(randomItemIndex);

                itemsToGenerate--;
            }

            for (int i = 0; i < _shelves.Length; i++)
            {
                var currentShelve = _shelves[i];

                currentShelve.SetItems(generatedItems.Take(currentShelve.Size).ToArray());
                generatedItems.RemoveRange(0, currentShelve.Size);
            }

            Debug.Assert(generatedItems.Count == 0, "Item generating for shelves is not working properly.");
        }
    }
}
