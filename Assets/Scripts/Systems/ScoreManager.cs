using UnityEngine;

namespace StreetEscape.Systems
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        private int _score;
        private int _highScore;
        private int _combo;

        public int Score => _score;
        public int HighScore => _highScore;
        public int Combo => _combo;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        public void AddScore(int points)
        {
            var multiplier = Mathf.Max(1, _combo);
            _score += points * multiplier;

            if (_score > _highScore)
            {
                _highScore = _score;
                PlayerPrefs.SetInt("HighScore", _highScore);
            }

            GameEvents.OnScoreChanged?.Invoke(_score);
        }

        public void IncrementCombo()
        {
            _combo++;
        }

        public void ResetCombo()
        {
            _combo = 0;
        }

        public void ResetScore()
        {
            _score = 0;
            _combo = 0;
            GameEvents.OnScoreChanged?.Invoke(_score);
        }
    }
}
