using UnityEngine;

namespace PotionsPlease.Util
{
    public static class PlayerPrefsHandler
    {
        public static bool ToggleMusic
        {
            get => PlayerPrefs.GetInt(nameof(ToggleMusic)) == 0;
            set => PlayerPrefs.SetInt(nameof(ToggleMusic), value ? 0 : 1); /// Inverted bool logic for default value to be "true"
        }

        public static bool ToggleSounds
        {
            get => PlayerPrefs.GetInt(nameof(ToggleSounds)) == 0;
            set => PlayerPrefs.SetInt(nameof(ToggleSounds), value ? 0 : 1); /// Inverted bool logic for default value to be "true"
        }

        public static int LastLevelIndex
        {
            get => PlayerPrefs.GetInt(nameof(LastLevelIndex));
            set => PlayerPrefs.SetInt(nameof(LastLevelIndex), Mathf.Min(Mathf.Max(LastLevelIndex, value), 9)); /// Capped at 3 (temporary)
        }

        public static int GetLevelStars(int levelIndex) => PlayerPrefs.GetInt($"Level{levelIndex}");
        public static void SetLevelStars(int levelIndex, int value) => PlayerPrefs.SetInt($"Level{levelIndex}", Mathf.Max(GetLevelStars(levelIndex), value));
    }
}
