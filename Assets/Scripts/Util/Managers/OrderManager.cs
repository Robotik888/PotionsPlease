using Cysharp.Threading.Tasks;
using PotionsPlease.InGame;
using PotionsPlease.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace PotionsPlease.Util.Managers
{
    public class OrderManager : ManagerBase<OrderManager>
    {
        [SerializeField] private OrderModel _tutorialOrder;
        [SerializeField] private ShelveObject[] _shelves;

        [Header("Level & Order")]
        private LevelModel _level;
        [SerializeField] public LevelModel EndLevel;
        private OrderModel _order;
        [SerializeField] private int _orderIndex = -1;

        [Header("Item generaton anim")]
        [SerializeField] private float _genAnimTimeOffset;
        [Space]
        [SerializeField] private float _genAnimInDuration;
        [SerializeField] private AnimationCurve _genAnimInCurve;
        [Space]
        [SerializeField] private float _genAnimOutDuration;
        [SerializeField] private AnimationCurve _genAnimOutCurve;

        private int _ratingTotal;

        private bool _isTutorial;

        public async void NextOrder()
        {
            string[] messageOrderOverride = null;
            
            if (_isTutorial)
            {
                if (_ratingTotal == 3)
                {
                    var message = ChatManager.Instance.GetOrderResponseByRating(_order, 3);
                    await ChatManager.Instance.ShowMessageUnclick(message);
                    await ChatManager.Instance.ShowProfileAnimAsync(false);

                    LevelComplete();
                    return;
                }

                /// First order try
                if (_ratingTotal == -1)
                {
                    _order = _tutorialOrder;
                }
                else
                {
                    messageOrderOverride = ChatManager.Instance.GetOrderResponseByRating(_order, _ratingTotal);
                }
            }
            else
            {
                _orderIndex++;

                if (_orderIndex == _level.Orders.Count)
                {
                    LevelComplete();
                    return;
                }

                _order = _level.Orders[_orderIndex];
            }

            //UIManager.Instance.RecipeInfoPanel.SetPotion(_order.Potion);
            SetShelfItemsActive(false);
            SetItemsDisable(true);

            await UniTask.Delay(500);

            ChatManager.Instance.SetCustomer(_order.Customer);

            await UniTask.WhenAll(
                (_orderIndex == 0 || _isTutorial && _ratingTotal == -1) ? UIManager.Instance.InGameDarken.SetDarkenAnimAsync(true) : UniTask.CompletedTask,
                (!_isTutorial || _isTutorial && _ratingTotal == -1) ? ChatManager.Instance.ShowProfileAnimAsync(true) : UniTask.CompletedTask
            );

            await ChatManager.Instance.ShowMessageOrder(messageOrderOverride ?? _order.MessagesOrder);

            await UIManager.Instance.InGameDarken.SetDarkenAnimAsync(false);
            
            await GenerateItemsAnim();
            

            SetShelfItemsActive(true);
        }

        public async void EndOrder(ItemModel[] cauldronItems)
        {
            // WITHOUT REVERSE FUNCTIONALITY
            //var rating = _order.PotionIngredients.Where(e => cauldronItems.Contains(e)).Count();
            int rating = 0;

            for (int index = 0; index < _order.PotionIngredients.Length; index++)
            {
                if (cauldronItems.Contains(_order.PotionIngredients[index]) && _order.PotionIngredients[index].IsReverted == _order.ReversedEffect[index])
                {
                    if (_level != EndLevel)
                    {
                        rating += 1;
                    }

                } else
                {
                    if (_level == EndLevel)
                    {
                        rating += 1;
                    }
                }
            }

            foreach(ShelveObject shelve in _shelves)
            {
                foreach(ItemObject obj in shelve.ItemObjects)
                {
                    obj.ItemModel.IsReverted = false;
                }
            }

            UIManager.Instance.ResetItemInfoText();
            SetShelfItemsActive(false);
            await ChatManager.Instance.HideOrderLastMessage();
            await GivePotionProcedureAsync();
            await UniTask.Delay(500);


            if (_isTutorial)
            {
                _ratingTotal = rating;
            }
            else
            {
                _ratingTotal += rating;
                await ChatManager.Instance.ShowMessageResponse(_order, rating);
                await UIManager.Instance.SetRatingTextAnimAsync(_ratingTotal);
                await ChatManager.Instance.ShowProfileAnimAsync(false);
            }

            NextOrder();
        }

        private void SetShelfItemsActive(bool value)
        {
            for (int i = 0; i < _shelves.Length; i++)
                _shelves[i].SetItemsActive(value);
        }

        public void SetItemsDisable(bool value)
        {
            foreach (var itemObject in _shelves.SelectMany(e => e.ItemObjects))
                itemObject.IsMenuDisable = value;
        }

        public async void StartLevel(int levelIndex)
        {
            if (levelIndex == 0)
            {
                _isTutorial = true;
                _ratingTotal = -1;
                _shelves = _shelves.SkipLast(1).ToArray(); // Set only 2 shelves for tutorial
            }
            else
            {
                _isTutorial = false;
                _level = GameManager.Instance.LevelsAll[levelIndex - 1];
            }

            await UniTask.Delay(1500);
            NextOrder();
        }

        private async UniTask GenerateItemsAnim()
        {
            var requiredItems = _order.PotionIngredients;
            var itemsToGenerate = _shelves.Sum(e => e.Size) - requiredItems.Length;

            var itemPool = GameManager.Instance.ItemsAll.Except(requiredItems).Except(_order.BannedIngredients).ToList();
            var generatedItems = new List<ItemModel>(requiredItems);

            /// Generate items
            while (itemsToGenerate != 0)
            {
                var randomItemIndex = Random.Range(0, itemPool.Count);

                generatedItems.Add(itemPool[randomItemIndex]);

                if (itemPool.Count > 1) /// Temporary solution for not running out of items in pool -> will duplicate last item util it fills the rest;
                    itemPool.RemoveAt(randomItemIndex);

                itemsToGenerate--;
            }

            /// Randomize order
            generatedItems = generatedItems.OrderBy(e => Random.value).ToList();

            SetItemsDisable(true);

            int i = 0;
            int lastItemIndex = _shelves.Sum(e => e.ItemObjects.Length) - 1;

            /// Populate shelves
            foreach (var item in _shelves.SelectMany(e => e.ItemObjects).Zip(generatedItems, (itemObject, itemModel) => new { ItemObject = itemObject, ItemModel = itemModel}))
            {
                var animTask = UniTask.Create(async () =>
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(_genAnimTimeOffset * (float)i));
                    await item.ItemObject.transform.LeanScale(Vector3.zero, _genAnimInDuration).setEase(_genAnimInCurve).ToUniTaskAsync();
                    item.ItemObject.ResetState(item.ItemModel);
                    await item.ItemObject.transform.LeanScale(Vector3.one * item.ItemObject.ShelfScale, _genAnimOutDuration).setEase(_genAnimOutCurve).ToUniTaskAsync();
                });

                if (i == lastItemIndex)
                    await animTask;
                else
                    i++;
            }

            SetItemsDisable(false);
        }   

        private async UniTask GivePotionProcedureAsync()
        {
            var potionOverlay = UIManager.Instance.PotionOverlay;

            potionOverlay.gameObject.SetActive(true);
            await potionOverlay.GivePotionProcedureAsync();
            potionOverlay.gameObject.SetActive(false);
        }

        private async void LevelComplete()
        {
            await UIManager.Instance.InGameDarken.SetDarkenAnimAsync(false);

            PlayerPrefsHandler.SetLevelStars(GameManager.Instance.LevelIndexCurrent, _ratingTotal);

            bool isLevelDone = _isTutorial || _level.GetLevelDoneByRating(_ratingTotal, _level == EndLevel);

            if (isLevelDone)
                PlayerPrefsHandler.LastLevelIndex = GameManager.Instance.LevelIndexCurrent + 1;

            MenuManager.Instance.UpdateMenuButtons();

            await UniTask.Delay(600);

            if (_isTutorial)
                MenuManager.Instance.ShowSummaryPanelAnim(_ratingTotal, 3, true);
            else
                MenuManager.Instance.ShowSummaryPanelAnim(_ratingTotal, _level.RatingMax, _level.GetLevelDoneByRating(_ratingTotal, _level==EndLevel));
        }




        private void Update()
        {
            if (Input.GetKeyDown((KeyCode.L)))
            {
                _ = GenerateItemsAnim();
            }
        }
    }
}
