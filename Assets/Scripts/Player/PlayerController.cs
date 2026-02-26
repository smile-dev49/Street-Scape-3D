using UnityEngine;
using StreetEscape.Core;
using StreetEscape.Managers;
using StreetEscape.Configs;

namespace StreetEscape.Player
{
    /// <summary>
    /// Controls player movement: lane-based forward/left/right.
    /// Uses Rigidbody for physics-based movement and collision detection.
    /// Requires: Rigidbody (kinematic or dynamic), Collider, Animator.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float laneSpacing = 2f;

        // State
        private int _currentLane;
        private int _currentStreet;
        private Vector3 _targetPosition;
        private bool _isDead;
        private bool _canMove;

        // Constants for input (mobile-friendly: tap left/right)
        private const string AnimatorDeathTrigger = "Death";
        private const string AnimatorJumpTrigger = "Jump";

        // ==================== PROPERTIES ====================

        public int CurrentLane => _currentLane;
        public int CurrentStreet => _currentStreet;
        public bool IsDead => _isDead;

        // ==================== UNITY LIFECYCLE ====================

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody>();
            if (animator == null) animator = GetComponentInChildren<Animator>();

            // Load config if available
            if (Managers.GameManager.Instance != null && Managers.GameManager.Instance.Config != null)
            {
                var config = Managers.GameManager.Instance.Config;
                moveSpeed = config.PlayerMoveSpeed;
                laneSpacing = config.LaneSpacing;
            }

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        private void Start()
        {
            _targetPosition = transform.position;
            _canMove = true;
        }

        private void OnEnable()
        {
            GameEvents.OnGameStarted += HandleGameStarted;
            GameEvents.OnGameOver += HandleGameOver;
            GameEvents.OnLevelRestart += HandleLevelRestart;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStarted -= HandleGameStarted;
            GameEvents.OnGameOver -= HandleGameOver;
            GameEvents.OnLevelRestart -= HandleLevelRestart;
        }

        private void Update()
        {
            if (!_canMove || _isDead) return;

            HandleInput();
        }

        private void FixedUpdate()
        {
            if (!_canMove || _isDead) return;

            MoveTowardsTarget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isDead) return;

            // Check collision with vehicle layer or tag
            if (other.CompareTag("Vehicle") || other.gameObject.layer == LayerMask.NameToLayer("Vehicle"))
            {
                Die();
            }
        }

        // ==================== INPUT ====================

        /// <summary>
        /// Handles input for lane switching and forward movement. Extend for mobile swipe/tap.
        /// Forward: Space, Up Arrow, or tap (mobile).
        /// </summary>
        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");

            if (horizontal > 0.1f)
            {
                MoveRight();
            }
            else if (horizontal < -0.1f)
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                MoveForward();
            }
        }

        // ==================== PUBLIC API ====================

        /// <summary>
        /// Move one lane to the left.
        /// </summary>
        public void MoveLeft()
        {
            if (_currentLane <= 0) return;

            _currentLane--;
            _targetPosition.x = GetLaneWorldPosition(_currentLane);
            GameEvents.RaisePlayerLaneChanged(_currentLane);

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger(AnimatorJumpTrigger);
            }
        }

        /// <summary>
        /// Move one lane to the right.
        /// </summary>
        public void MoveRight()
        {
            // Assume 3 lanes: -1, 0, 1 -> indices 0, 1, 2
            int maxLane = (GameManager.Instance?.Config?.LanesPerStreet ?? 3) - 1;
            if (_currentLane >= maxLane) return;

            _currentLane++;
            _targetPosition.x = GetLaneWorldPosition(_currentLane);
            GameEvents.RaisePlayerLaneChanged(_currentLane);

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger(AnimatorJumpTrigger);
            }
        }

        /// <summary>
        /// Move forward one step (call from external trigger when crossing street).
        /// </summary>
        public void MoveForward()
        {
            _targetPosition += Vector3.forward * laneSpacing;
            _currentStreet++;
            GameEvents.RaiseStreetCrossed(_currentStreet);
        }

        /// <summary>
        /// Initialize player at start position and lane.
        /// </summary>
        /// <param name="startPosition">World position.</param>
        /// <param name="startLane">Starting lane index (0-based).</param>
        public void Initialize(Vector3 startPosition, int startLane = 1)
        {
            _currentLane = startLane;
            _currentStreet = 0;
            _targetPosition = startPosition;
            transform.position = startPosition;
            _isDead = false;
            _canMove = true;
        }

        // ==================== MOVEMENT ====================

        private void MoveTowardsTarget()
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Lerp(pos.x, _targetPosition.x, Time.fixedDeltaTime * moveSpeed);
            pos.z = Mathf.Lerp(pos.z, _targetPosition.z, Time.fixedDeltaTime * moveSpeed);
            rb.MovePosition(pos);
        }

        private float GetLaneWorldPosition(int lane)
        {
            // Lanes centered: -1, 0, 1 for 3 lanes
            int centerLane = (GameManager.Instance?.Config?.LanesPerStreet ?? 3) / 2;
            return (lane - centerLane) * laneSpacing;
        }

        // ==================== DEATH ====================

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            _canMove = false;

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger(AnimatorDeathTrigger);
            }

            GameEvents.RaisePlayerDeath();
        }

        // ==================== EVENT HANDLERS ====================

        private void HandleGameStarted()
        {
            _canMove = true;
            _isDead = false;
        }

        private void HandleGameOver()
        {
            _canMove = false;
        }

        private void HandleLevelRestart()
        {
            _isDead = false;
            _canMove = true;
        }
    }
}
