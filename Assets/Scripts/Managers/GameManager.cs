using UnityEngine;
using StreetEscape.Core;
using StreetEscape.Configs;

namespace StreetEscape.Managers
{
    /// <summary>
    /// Singleton GameManager - central orchestrator for game state.
    /// Handles: Game flow, pause, game over, level transitions.
    /// Dependencies: LevelManager, CurrencyManager (injected or found).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private GameConfig gameConfig;

        [Header("State")]
        [SerializeField] private bool isPaused;
        [SerializeField] private bool isGameOver;
        [SerializeField] private bool isPlaying;

        // Cached references (lazy-loaded or assigned in Inspector)
        private LevelManager _levelManager;
        private CurrencyManager _currencyManager;

        // ==================== PROPERTIES ====================

        public bool IsPaused => isPaused;
        public bool IsGameOver => isGameOver;
        public bool IsPlaying => isPlaying;
        public GameConfig Config => gameConfig;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            // Singleton pattern - destroy duplicate instances
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CacheManagers();
        }

        private void OnEnable()
        {
            GameEvents.OnPlayerDeath += HandlePlayerDeath;
            GameEvents.OnLevelComplete += HandleLevelComplete;
        }

        private void OnDisable()
        {
            GameEvents.OnPlayerDeath -= HandlePlayerDeath;
            GameEvents.OnLevelComplete -= HandleLevelComplete;
        }

        // ==================== INITIALIZATION ====================

        /// <summary>
        /// Caches references to other managers. Call from Awake.
        /// </summary>
        private void CacheManagers()
        {
            _levelManager = FindFirstObjectByType<LevelManager>();
            _currencyManager = FindFirstObjectByType<CurrencyManager>();

            if (gameConfig == null)
            {
                Debug.LogWarning("[GameManager] GameConfig not assigned. Create one via Create > Street Escape > Game Config.");
            }
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Starts a new game (or level). Called from Main Menu or Level Select.
        /// </summary>
        public void StartGame()
        {
            isGameOver = false;
            isPaused = false;
            isPlaying = true;

            if (_levelManager != null)
            {
                _levelManager.LoadCurrentLevel();
            }

            GameEvents.RaiseGameStarted();
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void PauseGame()
        {
            if (isGameOver || !isPlaying) return;

            isPaused = true;
            Time.timeScale = 0f;
            GameEvents.RaiseGamePaused();
        }

        /// <summary>
        /// Resumes the game from pause.
        /// </summary>
        public void ResumeGame()
        {
            if (isGameOver || !isPlaying) return;

            isPaused = false;
            Time.timeScale = 1f;
            GameEvents.RaiseGameResumed();
        }

        /// <summary>
        /// Restarts the current level after death.
        /// </summary>
        public void RestartLevel()
        {
            isGameOver = false;
            isPaused = false;
            Time.timeScale = 1f;

            if (_levelManager != null)
            {
                _levelManager.RestartLevel();
            }

            GameEvents.RaiseLevelRestart();
        }

        /// <summary>
        /// Returns to main menu. Implement scene load as needed.
        /// </summary>
        public void ReturnToMenu()
        {
            Time.timeScale = 1f;
            isPlaying = false;
            isPaused = false;
            isGameOver = false;
            // TODO: Load main menu scene
        }

        // ==================== EVENT HANDLERS ====================

        private void HandlePlayerDeath()
        {
            isGameOver = true;
            isPlaying = false;
            GameEvents.RaiseGameOver();
        }

        private void HandleLevelComplete()
        {
            isPlaying = false;

            if (_levelManager != null)
            {
                _levelManager.CompleteLevel();
            }
        }
    }
}
