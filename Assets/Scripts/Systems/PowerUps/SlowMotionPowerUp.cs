using UnityEngine;

namespace StreetEscape.Systems.PowerUps
{
    [CreateAssetMenu(fileName = "SlowMotionPowerUp", menuName = "Street Escape/Power Ups/Slow Motion")]
    public class SlowMotionPowerUp : PowerUpBase
    {
        [SerializeField] private float timeScale = 0.5f;

        public override void Apply()
        {
            Time.timeScale = timeScale;
        }

        public override void Remove()
        {
            Time.timeScale = 1f;
        }
    }
}
