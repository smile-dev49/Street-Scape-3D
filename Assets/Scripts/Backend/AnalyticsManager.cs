using UnityEngine;

namespace StreetEscape.Backend
{
    public class AnalyticsManager : MonoBehaviour
    {
        public static AnalyticsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void TrackEvent(string eventName, string key = null, object value = null)
        {
            // TODO: Unity Analytics, Firebase, etc.
            Debug.Log($"[Analytics] {eventName}: {key}={value}");
        }

        public void TrackPurchase(string productId, decimal price)
        {
            TrackEvent("purchase", "product_id", productId);
        }

        public void TrackSessionStart()
        {
            TrackEvent("session_start");
        }

        public void TrackSessionEnd()
        {
            TrackEvent("session_end");
        }
    }
}
