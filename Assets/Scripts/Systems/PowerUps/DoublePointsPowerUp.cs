using UnityEngine;

namespace StreetEscape.Systems.PowerUps
{
    [CreateAssetMenu(fileName = "DoublePointsPowerUp", menuName = "Street Escape/Power Ups/Double Points")]
    public class DoublePointsPowerUp : PowerUpBase
    {
        private static bool _active;

        public static bool IsActive => _active;

        public override void Apply()
        {
            _active = true;
        }

        public override void Remove()
        {
            _active = false;
        }
    }
}
