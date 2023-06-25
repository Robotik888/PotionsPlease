using PotionsPlease.InGame;
using PotionsPlease.Util.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotionsPlease.Util.Managers
{
    public class MenuManager : ManagerBase<MenuManager>
    {
        [SerializeField] private TMP_Text _menuLevelNameText;
        [SerializeField] private Button _restartLevelButton;

        [Header("Popup menu")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _popupMenu;
        [SerializeField] private AnimationCurve _popupMenuShowAnimCurve;
        [SerializeField] private float _popupMenuShowAnimDuration;

        [Space]
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _soundsToggle;

        [Header("Summary panel")]
        [SerializeField] public CanvasGroup _summaryCanvasGroup;
        [SerializeField] public TMP_Text _summaryHeaderText;
        [SerializeField] public TMP_Text _summaryRatingText;
        [SerializeField] public TMP_Text _summaryRatingDescText;
        [SerializeField] public Button _summaryNextLevelButton;

        [Space]
        [SerializeField] private LevelButton[] _levelButtons;


        private void Start()
        {
            for (int i = 0; i < _levelButtons.Length; i++)
                _levelButtons[i].Initialize(i + 1);

            _popupMenu.SetActive(true);
            _popupMenu.transform.localScale = Vector3.zero;

            _musicToggle.onValueChanged.AddListener(ToggleMusic);
            _soundsToggle.onValueChanged.AddListener(ToggleSounds);

            /// Assigning saved values and also firing onValueChanged event
            /// that sets the AudioSystem accordingly
            _musicToggle.isOn = PlayerPrefsHandler.ToggleMusic;
            _soundsToggle.isOn = PlayerPrefsHandler.ToggleSounds;


            if (UIManager.IsStartLevelOverride)
            {
                _restartLevelButton.interactable = true;
                _menuPanel.SetActive(false);
            }
            else
            {
                _menuLevelNameText.text = GetLevelName(PlayerPrefsHandler.LastLevelIndex, false);
            }
        }

        public void ShowPopupMenu(bool value)
        {
            /// Prevent unmuting menu soundtrack in menu
            if (GameManager.Instance.LevelIndexCurrent != -1)
                AudioSystem.Instance.SetSoundtrackMenuState(value);

            if (LeanTween.isTweening(_popupMenu))
                return;

            OrderManager.Instance.SetItemsDisable(value);

            if (value)
            {
                _popupMenu.SetActive(true);
                _popupMenu
                    .LeanScale(Vector3.one, _popupMenuShowAnimDuration)
                    .setEase(_popupMenuShowAnimCurve);
            }   
            else
            {
                _popupMenu
                    .LeanScale(Vector3.zero, _popupMenuShowAnimDuration)
                    .setEase(LeanTweenType.easeOutQuart)
                    .setOnComplete(() => _popupMenu.SetActive(false));
            }
        }

        public void ToggleMusic(bool value)
        {
            PlayerPrefsHandler.ToggleMusic = value;
            AudioSystem.Instance.ToggleMusic(value);
        }

        public void ToggleSounds(bool value)
        {
            PlayerPrefsHandler.ToggleSounds = value;
            AudioSystem.Instance.ToggleSounds(value);
        }

        public void UpdateMenuButtons()
        {
            for (int i = 0; i < _levelButtons.Length; i++)
                _levelButtons[i].UpdateState();
        }

        public void PlayLevel(int levelIndex) {
            UIManager.Instance.StartLevelBegin(levelIndex);
        } 

        public void PlayButtonClick() => PlayLevel(PlayerPrefsHandler.LastLevelIndex);
        public void TutorialButtonClick() => PlayLevel(0);
        public void RestartLevelButtonClick() => PlayLevel(GameManager.Instance.LevelIndexCurrent);

        public string GetLevelName(int levelIndex, bool isForNewLevel)
        {
            string FormatFirstLine(string text) => isForNewLevel ? $"<size=135%><b>{text}</b></size>" : $"<b>{text}</b>";

            string textBetween = isForNewLevel ? "\n" : ": ";

            if (levelIndex == 0)
                return FormatFirstLine("Tutorial");
            else
                return $"{FormatFirstLine($"Day {levelIndex}")}{textBetween}{GameManager.Instance.LevelsAll[levelIndex - 1].Name}";
        }

        public async void ShowSummaryPanelAnim(int rating, int ratingMax, bool isLevelDone)
        {
            UIManager.Instance.HideCanvasGroupAnim();
            AudioSystem.Instance.SetSoundtrackMenuState(true);

            _summaryCanvasGroup.alpha = 0;
            _summaryCanvasGroup.gameObject.SetActive(true);
            _summaryCanvasGroup.blocksRaycasts = true;

            _summaryRatingText.text = $"{rating}/{ratingMax} {UIManager.SPRITE_STAR}";

            var levelIndex = GameManager.Instance.LevelIndexCurrent;
            _summaryHeaderText.text = $"{(levelIndex == 0 ? "Tutorial" : $"Day {levelIndex}")}\ncompleted";

            _summaryNextLevelButton.interactable = isLevelDone;

            _summaryRatingDescText.text = isLevelDone ? string.Empty : $"You need at least {ratingMax - 2} {UIManager.SPRITE_STAR} to continue.";

            await UIManager.Instance.InGameDarken.SetDarkenAnimAsync(true);
            await _summaryCanvasGroup.LeanAlpha(1, 0.5f).setEaseOutQuint().ToUniTaskAsync();
        }

        public void SummaryNextLevelClick()
        {
            /// Capped at 3 for demo
            var nextLevel = Mathf.Min(GameManager.Instance.LevelIndexCurrent + 1, 8);
            PlayLevel(nextLevel);
        }
    }
}
