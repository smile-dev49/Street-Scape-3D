using UnityEngine;
using UnityEngine.UI;
using StreetEscape.Systems;
using StreetEscape.Backend;

namespace StreetEscape.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button leaderboardButton;

        [Header("UI")]
        [SerializeField] private Text highScoreText;

        private void OnEnable()
        {
            if (playButton != null)
                playButton.onClick.AddListener(OnPlay);
            if (shopButton != null)
                shopButton.onClick.AddListener(OnShop);
            if (leaderboardButton != null)
                leaderboardButton.onClick.AddListener(OnLeaderboard);

            if (highScoreText != null && ScoreManager.Instance != null)
                highScoreText.text = $"Best: {ScoreManager.Instance.HighScore}";
        }

        private void OnDisable()
        {
            if (playButton != null)
                playButton.onClick.RemoveListener(OnPlay);
            if (shopButton != null)
                shopButton.onClick.RemoveListener(OnShop);
            if (leaderboardButton != null)
                leaderboardButton.onClick.RemoveListener(OnLeaderboard);
        }

        private void OnPlay()
        {
            GameManager.Instance?.StartGame();
        }

        private void OnShop()
        {
            FindObjectOfType<UIManager>()?.ShowShopPanel();
        }

        private void OnLeaderboard()
        {
            LeaderboardManager.Instance?.ShowLeaderboard();
        }
    }
}
