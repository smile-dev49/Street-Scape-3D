using System;

namespace StreetEscape.Systems
{
    public static class GameEvents
    {
        public static event Action<int> OnScoreChanged;
        public static event Action OnPlayerHit;
        public static event Action OnGameOver;
        public static event Action OnPowerUpCollected;
        public static event Action<string> OnPurchaseComplete;
    }
}
