using PotionsPlease.Util;
using PotionsPlease.Util.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PotionsPlease.InGame
{
    public class DebugControlButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
    {
        private enum ActionType
        {
            Reset,
            UnlockLevels
        }

        [SerializeField] private ActionType _actionType;

        public void OnPointerDown(PointerEventData eventData) => SetRestartButton(true);
        public void OnPointerClick(PointerEventData eventData) => SetRestartButton(false);
        public void OnPointerExit(PointerEventData eventData) => SetRestartButton(false);


        private void SetRestartButton(bool value)
        {
            if (value)
                LeanTween.delayedCall(gameObject, 3, () =>
                {
                    switch (_actionType)
                    {
                        case ActionType.Reset:
                            PlayerPrefs.DeleteAll();
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            break;
                        case ActionType.UnlockLevels:
                            PlayerPrefsHandler.SetLevelStars(0, 3);
                            PlayerPrefsHandler.SetLevelStars(1, 9);
                            PlayerPrefsHandler.SetLevelStars(2, 9);
                            PlayerPrefsHandler.SetLevelStars(3, 12);
                            PlayerPrefsHandler.LastLevelIndex = 3;
                            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                            //MenuManager.Instance.UpdateMenuButtons();
                            break;
                    }
                });
            else
                LeanTween.cancel(gameObject);
        }
    }
}
