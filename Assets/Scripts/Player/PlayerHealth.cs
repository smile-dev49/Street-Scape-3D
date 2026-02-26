using UnityEngine;
using StreetEscape.Systems;

namespace StreetEscape.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int maxLives = 3;
        [SerializeField] private float invincibilityDuration = 2f;

        [Header("References")]
        [SerializeField] private PlayerModel playerModel;

        private int _currentLives;
        private bool _isInvincible;
        private float _invincibilityTimer;

        public int CurrentLives => _currentLives;
        public bool IsInvincible => _isInvincible;

        private void Start()
        {
            _currentLives = maxLives;
        }

        private void Update()
        {
            if (_isInvincible)
            {
                _invincibilityTimer -= Time.deltaTime;
                if (_invincibilityTimer <= 0f)
                    _isInvincible = false;
            }
        }

        public void TakeDamage()
        {
            if (_isInvincible) return;

            _currentLives--;
            _isInvincible = true;
            _invincibilityTimer = invincibilityDuration;

            playerModel?.TriggerHit();

            GameEvents.OnPlayerHit?.Invoke();

            if (_currentLives <= 0)
                GameEvents.OnGameOver?.Invoke();
        }

        public void Heal(int amount = 1)
        {
            _currentLives = Mathf.Min(_currentLives + amount, maxLives);
        }

        public void ResetHealth()
        {
            _currentLives = maxLives;
            _isInvincible = false;
        }
    }
}
