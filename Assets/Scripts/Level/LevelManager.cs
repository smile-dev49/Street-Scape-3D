using UnityEngine;
using StreetEscape.Core;
using StreetEscape.Configs;
using StreetEscape.Managers;

namespace StreetEscape.Level
{
    /// <summary>
    /// Manages level progression: street count, difficulty scaling.
    /// Handles restart on death, unlock next level on success.
    /// Saves current level to PlayerPrefs.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Street streetPrefab;
        [SerializeField] private Transform streetContainer;
        [SerializeField] private Transform levelStartPoint;

        [Header("Player")]
        [SerializeField] private Player.PlayerController playerController;

        private const string CurrentLevelKey = "StreetEscape_CurrentLevel";
        private const string UnlockedLevelKey = "StreetEscape_UnlockedLevel";

        private int _currentLevel = 1;
        private int _unlockedLevel = 1;
        private int _streetsToCross;
        private int _streetsCrossed;
        private Street[] _streets;

        // ==================== PROPERTIES ====================

        public int CurrentLevel => _currentLevel;
        public int UnlockedLevel => _unlockedLevel;
        public int StreetsToCross => _streetsToCross;
        public int StreetsCrossed => _streetsCrossed;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            LoadProgress();
        }

        private void OnEnable()
        {
            GameEvents.OnStreetCrossed += HandleStreetCrossed;
        }

        private void OnDisable()
        {
            GameEvents.OnStreetCrossed -= HandleStreetCrossed;
        }

        private void Start()
        {
            // Level is loaded when GameManager.StartGame() is called
        }

        // ==================== PERSISTENCE ====================

        private void LoadProgress()
        {
            _currentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
            _unlockedLevel = PlayerPrefs.GetInt(UnlockedLevelKey, 1);
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetInt(CurrentLevelKey, _currentLevel);
            PlayerPrefs.SetInt(UnlockedLevelKey, _unlockedLevel);
            PlayerPrefs.Save();
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Load and build the current level.
        /// </summary>
        public void LoadCurrentLevel()
        {
            if (gameConfig == null)
            {
                Debug.LogError("[LevelManager] GameConfig not assigned.");
                return;
            }

            ClearLevel();

            _streetsToCross = gameConfig.BaseStreetsCount + (_currentLevel - 1) * gameConfig.StreetsPerLevel;
            _streetsCrossed = 0;

            BuildLevel();

            if (playerController != null && levelStartPoint != null)
            {
                Vector3 startPos = levelStartPoint.position;
                int startLane = gameConfig.LanesPerStreet / 2;
                playerController.Initialize(startPos, startLane);
            }

            GameEvents.RaiseLevelChanged(_currentLevel);
        }

        /// <summary>
        /// Restart the current level after death.
        /// </summary>
        public void RestartLevel()
        {
            LoadCurrentLevel();
        }

        /// <summary>
        /// Called when level is completed successfully.
        /// </summary>
        public void CompleteLevel()
        {
            if (_currentLevel >= _unlockedLevel)
            {
                _unlockedLevel = _currentLevel + 1;
            }

            SaveProgress();
            GameEvents.RaiseLevelComplete();
        }

        /// <summary>
        /// Load a specific level (e.g. from level select).
        /// </summary>
        public void LoadLevel(int level)
        {
            if (level < 1 || level > _unlockedLevel)
            {
                Debug.LogWarning($"[LevelManager] Level {level} not unlocked. Unlocked: {_unlockedLevel}");
                return;
            }

            _currentLevel = level;
            LoadCurrentLevel();
        }

        // ==================== LEVEL BUILDING ====================

        private void BuildLevel()
        {
            if (streetPrefab == null || streetContainer == null)
            {
                Debug.LogWarning("[LevelManager] Street prefab or container not assigned. Level will be empty.");
                return;
            }

            _streets = new Street[_streetsToCross];
            float streetSpacing = gameConfig.LaneSpacing * 2f;

            for (int i = 0; i < _streetsToCross; i++)
            {
                Vector3 pos = streetContainer.position + Vector3.forward * (i * streetSpacing);
                Street street = Instantiate(streetPrefab, pos, Quaternion.identity, streetContainer);
                street.Initialize(i, _currentLevel);
                _streets[i] = street;
            }
        }

        private void ClearLevel()
        {
            if (_streets != null)
            {
                foreach (Street s in _streets)
                {
                    if (s != null) Destroy(s.gameObject);
                }
                _streets = null;
            }

            if (streetContainer != null)
            {
                foreach (Transform child in streetContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        // ==================== EVENT HANDLERS ====================

        private void HandleStreetCrossed(int streetIndex)
        {
            _streetsCrossed = streetIndex;

            if (_streetsCrossed >= _streetsToCross)
            {
                GameEvents.RaiseLevelComplete();
            }
        }
    }
}
