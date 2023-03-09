using PotionsPlease.InGame;
using PotionsPlease.Models;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class GameManager : ManagerBase<GameManager>
    {
        public static Camera CameraMain => Static.GetComponentByNameProperty(ref _cameraMain, "Main Camera");
        private static Camera _cameraMain;

        public int LevelIndexCurrent { get; set; } = -1;
        public ItemObject DraggedItemCurrent { get; set; }

        [field: SerializeField] public CauldronObject Cauldron { get; private set; }
        [field: SerializeField] public DropAreaObject DropAreaObject { get; private set; }

        /// Temporary solution for holding all item and potion models; probably will be changed later to load from resources
        [field: SerializeField] public LevelModel[] LevelsAll { get; private set; }
        [field: SerializeField] public ItemModel[] ItemsAll { get; private set; }

        private void Start()
        {
            if (LevelIndexCurrent == -1)
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 120;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            _cameraMain = Camera.main;

            //await UniTask.Delay(500);
            //await UIManager.Instance.StartGameBackgroundAnimAsync();
            //await UniTask.Delay(1000);
            //OrderManager.Instance.NextOrder();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LeanTween.cancelAll();
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }



        public void StartLevel(int levelIndex)
        {
            LevelIndexCurrent = levelIndex;
            OrderManager.Instance.StartLevel(levelIndex);
        }
    }
}