using UnityEngine;
using UnityEngine.UI;
using StreetEscape.Systems;

namespace StreetEscape.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        private void OnEnable()
        {
            if (ScoreManager.Instance != null)
            {
                if (scoreText != null)
                    scoreText.text = ScoreManager.Instance.Score.ToString();
                if (highScoreText != null)
                    highScoreText.text = ScoreManager.Instance.HighScore.ToString();
            }

            if (retryButton != null)
                retryButton.onClick.AddListener(OnRetry);
            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenu);
        }

        private void OnDisable()
        {
            if (retryButton != null)
                retryButton.onClick.RemoveListener(OnRetry);
            if (menuButton != null)
                menuButton.onClick.RemoveListener(OnMenu);
        }

        private void OnRetry()
        {
            GameManager.Instance?.StartGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }

        private void OnMenu()
        {
            GameManager.Instance?.ReturnToMenu();
        }
    }
}
