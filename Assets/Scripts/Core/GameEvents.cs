using System;
using UnityEngine;

namespace StreetEscape.Core
{
    /// <summary>
    /// Central event system for decoupling game systems.
    /// Use events instead of direct references to avoid tight coupling.
    /// Subscribe/Unsubscribe in OnEnable/OnDisable to prevent memory leaks.
    /// </summary>
    public static class GameEvents
    {
        // ==================== GAME STATE ====================

        /// <summary>Fired when the game starts (level begins).</summary>
        public static event Action OnGameStarted;

        /// <summary>Fired when the game pauses.</summary>
        public static event Action OnGamePaused;

        /// <summary>Fired when the game resumes from pause.</summary>
        public static event Action OnGameResumed;

        /// <summary>Fired when the game ends (game over).</summary>
        public static event Action OnGameOver;

        /// <summary>Fired when the level is completed successfully.</summary>
        public static event Action OnLevelComplete;

        /// <summary>Fired when the player dies (collision with vehicle).</summary>
        public static event Action OnPlayerDeath;

        /// <summary>Fired when the level restarts after death.</summary>
        public static event Action OnLevelRestart;

        // ==================== PLAYER ====================

        /// <summary>Fired when the player crosses a street. Param: street index.</summary>
        public static event Action<int> OnStreetCrossed;

        /// <summary>Fired when the player moves to a new lane. Param: lane index.</summary>
        public static event Action<int> OnPlayerLaneChanged;

        // ==================== ECONOMY ====================

        /// <summary>Fired when coins change. Param: new coin amount.</summary>
        public static event Action<int> OnCoinsChanged;

        /// <summary>Fired when coins are earned. Param: amount earned.</summary>
        public static event Action<int> OnCoinsEarned;

        // ==================== LEVEL ====================

        /// <summary>Fired when the level changes. Param: new level number.</summary>
        public static event Action<int> OnLevelChanged;

        // ==================== INVOKE METHODS ====================

        public static void RaiseGameStarted() => OnGameStarted?.Invoke();
        public static void RaiseGamePaused() => OnGamePaused?.Invoke();
        public static void RaiseGameResumed() => OnGameResumed?.Invoke();
        public static void RaiseGameOver() => OnGameOver?.Invoke();
        public static void RaiseLevelComplete() => OnLevelComplete?.Invoke();
        public static void RaisePlayerDeath() => OnPlayerDeath?.Invoke();
        public static void RaiseLevelRestart() => OnLevelRestart?.Invoke();
        public static void RaiseStreetCrossed(int streetIndex) => OnStreetCrossed?.Invoke(streetIndex);
        public static void RaisePlayerLaneChanged(int laneIndex) => OnPlayerLaneChanged?.Invoke(laneIndex);
        public static void RaiseCoinsChanged(int amount) => OnCoinsChanged?.Invoke(amount);
        public static void RaiseCoinsEarned(int amount) => OnCoinsEarned?.Invoke(amount);
        public static void RaiseLevelChanged(int level) => OnLevelChanged?.Invoke(level);
    }
}
