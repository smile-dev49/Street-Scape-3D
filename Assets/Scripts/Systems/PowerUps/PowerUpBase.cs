using UnityEngine;

namespace StreetEscape.Systems.PowerUps
{
    public abstract class PowerUpBase : ScriptableObject
    {
        [SerializeField] protected string id;
        [SerializeField] protected string displayName;
        [SerializeField] protected Sprite icon;

        public string Id => id;
        public string DisplayName => displayName;
        public Sprite Icon => icon;

        public abstract void Apply();
        public abstract void Remove();
    }
}
