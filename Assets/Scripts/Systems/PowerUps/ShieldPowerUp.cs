using UnityEngine;

namespace StreetEscape.Systems.PowerUps
{
    [CreateAssetMenu(fileName = "ShieldPowerUp", menuName = "Street Escape/Power Ups/Shield")]
    public class ShieldPowerUp : PowerUpBase
    {
        private static bool _shieldActive;

        public static bool IsActive => _shieldActive;

        public override void Apply()
        {
            _shieldActive = true;
        }

        public override void Remove()
        {
            _shieldActive = false;
        }
    }
}
