using UnityEngine;
using UnityEngine.UI;
using StreetEscape.Systems;

namespace StreetEscape.UI
{
    public class HUDController : MonoBehaviour
    {
        [Header("Score")]
        [SerializeField] private Text scoreText;

        [Header("Lives")]
        [SerializeField] private GameObject[] lifeIcons;

        private void OnEnable()
        {
            GameEvents.OnScoreChanged += UpdateScore;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreChanged -= UpdateScore;
        }

        private void Start()
        {
            UpdateScore(ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0);
        }

        private void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        public void UpdateLives(int currentLives)
        {
            if (lifeIcons == null) return;

            for (var i = 0; i < lifeIcons.Length; i++)
                if (lifeIcons[i] != null)
                    lifeIcons[i].SetActive(i < currentLives);
        }
    }
}
