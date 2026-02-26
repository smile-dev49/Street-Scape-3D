using UnityEngine;

namespace StreetEscape.Vehicles
{
    /// <summary>
    /// Base component for all vehicles (Car, Bus, Motorcycle, FastCar).
    /// Handles movement and pooling lifecycle.
    /// Requires: Rigidbody or Transform movement, Collider with "Vehicle" tag.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Vehicle : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector3 moveDirection = Vector3.right;

        [Header("Pooling")]
        [SerializeField] private bool useObjectPool = true;

        private Transform _transform;
        private Rigidbody _rb;
        private bool _isMoving;

        // ==================== PROPERTIES ====================

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public Vector3 MoveDirection
        {
            get => moveDirection;
            set => moveDirection = value.normalized;
        }

        public bool IsMoving => _isMoving;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            _transform = transform;
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_isMoving) return;

            if (_rb != null && !_rb.isKinematic)
            {
                _rb.linearVelocity = moveDirection * speed;
            }
            else
            {
                _transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
            }
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Activate vehicle with given speed and direction.
        /// </summary>
        public void Activate(float vehicleSpeed, Vector3 direction)
        {
            speed = vehicleSpeed;
            moveDirection = direction.normalized;
            _isMoving = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivate and return to pool if applicable.
        /// </summary>
        public void Deactivate()
        {
            _isMoving = false;

            if (useObjectPool)
            {
                var pool = VehiclePool.Instance;
                if (pool != null)
                {
                    pool.ReturnToPool(this);
                    return;
                }
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when vehicle exits the play area (despawn).
        /// </summary>
        public void OnExitBounds()
        {
            Deactivate();
        }
    }
}
