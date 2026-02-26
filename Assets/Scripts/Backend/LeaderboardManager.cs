using UnityEngine;
using StreetEscape.Systems;
using System;
using System.Collections.Generic;

namespace StreetEscape.Backend
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance { get; private set; }

        private const string LeaderboardKey = "Leaderboard";
        private const int MaxLocalEntries = 100;

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

        public void SubmitScore(int score)
        {
            if (ScoreManager.Instance == null) return;

            var id = SystemInfo.deviceUniqueIdentifier;
            var name = "Player"; // TODO: Get from player prefs or auth

            // TODO: Submit to PlayFab, Firebase, or custom backend
            SaveLocalScore(id, name, score);
        }

        private void SaveLocalScore(string id, string name, int score)
        {
            var json = PlayerPrefs.GetString(LeaderboardKey, "[]");
            var entries = JsonUtility.FromJson<LeaderboardList>(json);
            if (entries == null) entries = new LeaderboardList { entries = new List<LeaderboardEntry>() };

            entries.entries.Add(new LeaderboardEntry { id = id, name = name, score = score });
            entries.entries.Sort((a, b) => b.score.CompareTo(a.score));

            if (entries.entries.Count > MaxLocalEntries)
                entries.entries.RemoveRange(MaxLocalEntries, entries.entries.Count - MaxLocalEntries);

            PlayerPrefs.SetString(LeaderboardKey, JsonUtility.ToJson(entries));
        }

        public void FetchLeaderboard(System.Action<List<LeaderboardEntry>> onComplete)
        {
            // TODO: Fetch from backend
            var json = PlayerPrefs.GetString(LeaderboardKey, "{\"entries\":[]}");
            var list = JsonUtility.FromJson<LeaderboardList>(json);
            onComplete?.Invoke(list?.entries ?? new List<LeaderboardEntry>());
        }

        public void ShowLeaderboard()
        {
            // TODO: Open leaderboard UI or overlay
            FetchLeaderboard(entries =>
            {
                foreach (var e in entries)
                    Debug.Log($"{e.name}: {e.score}");
            });
        }

        [Serializable]
        public class LeaderboardEntry
        {
            public string id;
            public string name;
            public int score;
        }

        [Serializable]
        private class LeaderboardList
        {
            public List<LeaderboardEntry> entries;
        }
    }
}
