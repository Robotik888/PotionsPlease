using PotionsPlease.Models;
using PotionsPlease.Util;
using PotionsPlease.Util.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _starsText;

        public object PlayerPrefsHander { get; private set; }

        private int _levelIndex;
        private LevelModel _level;

        public void Initialize(int levelIndex)
        {
            _button.onClick.AddListener(() => MenuManager.Instance.PlayLevel(levelIndex));
            _levelIndex = levelIndex;
            _level = GameManager.Instance.LevelsAll[_levelIndex - 1];
            _levelText.text = levelIndex.ToString();
            UpdateState();
        }

        public void UpdateState()
        {
            int stars = PlayerPrefsHandler.GetLevelStars(_levelIndex);
            int starsMax = _level.Orders.Count * 3;

            _starsText.text = $"{stars}/{starsMax} {UIManager.SPRITE_STAR}";

            int prevLevelStars = PlayerPrefsHandler.GetLevelStars(_levelIndex - 1);

            _button.interactable = _levelIndex == 1 ? prevLevelStars == 3 : GameManager.Instance.LevelsAll[_levelIndex - 2].GetLevelDoneByRating(prevLevelStars);
        }
    }
}
