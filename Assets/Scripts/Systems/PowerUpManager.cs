using UnityEngine;
using System.Collections.Generic;

namespace StreetEscape.Systems
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        private readonly Dictionary<string, PowerUps.PowerUpBase> _activePowerUps = new();
        private readonly Dictionary<string, float> _timers = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            var toRemove = new List<string>();
            foreach (var kvp in _timers)
            {
                _timers[kvp.Key] -= Time.deltaTime;
                if (_timers[kvp.Key] <= 0f)
                    toRemove.Add(kvp.Key);
            }

            foreach (var id in toRemove)
                RemovePowerUp(id);
        }

        public void ApplyPowerUp(PowerUps.PowerUpBase powerUp, float duration)
        {
            var id = powerUp.Id;
            if (_activePowerUps.ContainsKey(id))
            {
                _activePowerUps[id].Remove();
                _activePowerUps.Remove(id);
                _timers.Remove(id);
            }

            powerUp.Apply();
            _activePowerUps[id] = powerUp;
            _timers[id] = duration;

            GameEvents.OnPowerUpCollected?.Invoke();
        }

        private void RemovePowerUp(string id)
        {
            if (_activePowerUps.TryGetValue(id, out var powerUp))
            {
                powerUp.Remove();
                _activePowerUps.Remove(id);
            }
            _timers.Remove(id);
        }

        public bool HasPowerUp(string id) => _activePowerUps.ContainsKey(id);
    }
}
