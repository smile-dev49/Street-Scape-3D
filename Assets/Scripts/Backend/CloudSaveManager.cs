using UnityEngine;

namespace StreetEscape.Backend
{
    public class CloudSaveManager : MonoBehaviour
    {
        public static CloudSaveManager Instance { get; private set; }

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

        public void SaveToCloud()
        {
            // TODO: Push local save to PlayFab / Firebase
        }

        public void LoadFromCloud()
        {
            // TODO: Pull cloud save, merge with local
        }
    }
}
