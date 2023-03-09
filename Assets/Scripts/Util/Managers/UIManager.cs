using Cysharp.Threading.Tasks;
using PotionsPlease.Models;
using PotionsPlease.Util;
using PotionsPlease.Util.Managers;
using PotionsPlease.Util.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PotionsPlease.InGame
{
    public class UIManager : ManagerBase<UIManager>
    {
        public const string SPRITE_STAR = "<sprite name=star>";

        public static bool IsStartLevelOverride => _awakeStartLevel != -1;

        private static int _awakeStartLevel = -1;

        [SerializeField] public TMP_Text _itemInfoHeaderText;
        [SerializeField] public TMP_Text _itemInfoDescText;
        [SerializeField] public TMP_Text _ratingText;
        [SerializeField] public CanvasGroup _ratingBarCanvasGroup;
        [SerializeField] public TMP_Text _levelNameText;

        [field: Space]
        [field: SerializeField] public RecipeInfoPanel RecipeInfoPanel { get; private set; }
        [field: SerializeField] public BackgroundDarken InGameDarken { get; private set; }
        [field: SerializeField] public BackgroundDarken GlobalDarken { get; private set; }
        [field: SerializeField] public PotionOverlay PotionOverlay { get; private set; }

        [Header("Start level anim")]
        [SerializeField] private float _startLevelAnimTextDuration;
        [SerializeField] private AnimationCurve _startLevelAnimTextCurve;

        protected override void Awake()
        {
            base.Awake();

            GlobalDarken.BackgroundImage.color = Color.black;
            GlobalDarken.gameObject.SetActive(true);
        }

        private async void Start()
        {
            if (IsStartLevelOverride)
                StartLevelEnd();
            else
            {
                AudioSystem.Instance.SetSoundtrackMenuState(true, true);
                _ratingBarCanvasGroup.gameObject.SetActive(false);
                await GlobalDarken.SetDarkenAnimAsync(false);
                GlobalDarken.gameObject.SetActive(false);
            }
        }

        public async void StartLevelBegin(int levelIndex)
        {
            LeanTween.cancelAll();

            OrderManager.Instance.SetItemsDisable(true);

            GlobalDarken.gameObject.SetActive(true);
            await GlobalDarken.SetDarkenAnimAsync(true);
            _awakeStartLevel = levelIndex;
            SceneManager.LoadScene(0);
        }
        
        private async void StartLevelEnd()
        {
            AudioSystem.Instance.SetSoundtrackMenuState(true, true);

            _levelNameText.text = MenuManager.Instance.GetLevelName(_awakeStartLevel, true);
            await LeanTween.value(_levelNameText.gameObject, _levelNameText.color.SetA(0), _levelNameText.color.SetA(1), _startLevelAnimTextDuration)
                .setOnUpdate((Color color) => _levelNameText.color = color)
                .setEase(_startLevelAnimTextCurve)
                .ToUniTaskAsync();

            AudioSystem.Instance.SetSoundtrackMenuState(false);
            await GlobalDarken.SetDarkenAnimAsync(false);

            GlobalDarken.gameObject.SetActive(false);
            OrderManager.Instance.SetItemsDisable(true);

            GameManager.Instance.StartLevel(_awakeStartLevel);

            _awakeStartLevel = -1;
        }

        public void SetItemInfoText(ItemModel itemModel)
        {
            _itemInfoHeaderText.text = itemModel.Name;
            _itemInfoDescText.text = itemModel.Description;
        }
        
        public void ResetItemInfoText()
        {
            _itemInfoHeaderText.text = string.Empty;
            _itemInfoDescText.text   = string.Empty;
        }

        public async UniTask SetRatingTextAnimAsync(int rating)
        {
            await _ratingText.transform.LeanScale(Vector3.one * 1.2f, 0.3f).setEase(LeanTweenType.easeInOutCirc).ToUniTaskAsync();
            _ratingText.text = $"{rating} {SPRITE_STAR}";
            await _ratingText.transform.LeanScale(Vector3.one * 1.0f, 0.3f).setEase(LeanTweenType.easeInOutCirc).ToUniTaskAsync();
        }

        public void HideCanvasGroupAnim() => _ratingBarCanvasGroup.LeanAlpha(0, 0.5f).setEaseOutCirc();
    }
}
