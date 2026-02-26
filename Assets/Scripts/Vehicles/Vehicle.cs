using UnityEngine;
using StreetEscape.Player;

namespace StreetEscape.Vehicles
{
    [RequireComponent(typeof(Collider))]
    public class Vehicle : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private Vector3 moveDirection = Vector3.back;

        [Header("Optional")]
        [SerializeField] private bool useLocalDirection;

        private bool _isActive;

        private void Update()
        {
            if (!_isActive) return;

            var dir = useLocalDirection ? transform.TransformDirection(moveDirection) : moveDirection;
            transform.position += dir.normalized * (speed * Time.deltaTime);
        }

        public void Activate(Vector3 position)
        {
            transform.position = position;
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<PlayerHealth>();
                health?.TakeDamage();
            }
        }
    }
}
