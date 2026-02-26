using UnityEngine;

namespace StreetEscape.Player
{
    /// <summary>
    /// Crossy Road–style grid-based controller. Moves forward, left, or right one cell per input.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private Vector3 gridForward = Vector3.forward;
        [SerializeField] private Vector3 gridRight = Vector3.right;

        [Header("Movement")]
        [SerializeField] private float moveDuration = 0.15f;

        private Vector3 _targetWorldPos;
        private Vector3 _moveStartPos;
        private float _moveT;
        private bool _isMoving;

        private void Start()
        {
            _targetWorldPos = transform.position;
        }

        private void Update()
        {
            PollInput();
            if (_isMoving)
                UpdateMove();
        }

        /// <summary>Reads input and queues one move per press.</summary>
        private void PollInput()
        {
            if (_isMoving) return;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                MoveInDirection(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                MoveInDirection(-1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                MoveInDirection(-1, true);
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                MoveInDirection(1, true);
        }

        /// <summary>Moves one cell along forward (dir 1/-1) or sideways (axisSideways true).</summary>
        private void MoveInDirection(int dir, bool axisSideways = false)
        {
            var delta = axisSideways ? gridRight * (dir * cellSize) : gridForward * (dir * cellSize);

            _moveStartPos = _targetWorldPos;
            _targetWorldPos += delta;
            _moveT = 0f;
            _isMoving = true;
        }

        /// <summary>Advances the smooth step toward the target position.</summary>
        private void UpdateMove()
        {
            _moveT += Time.deltaTime / moveDuration;
            if (_moveT >= 1f)
            {
                transform.position = _targetWorldPos;
                _isMoving = false;
                return;
            }

            var t = Mathf.SmoothStep(0f, 1f, _moveT);
            transform.position = Vector3.Lerp(_moveStartPos, _targetWorldPos, t);
        }

        /// <summary>Returns the current grid cell in local grid coordinates.</summary>
        public Vector3 GetGridPosition() => _targetWorldPos;

        /// <summary>Snaps the player to a world position (e.g. on spawn or teleport).</summary>
        public void SnapToPosition(Vector3 worldPos)
        {
            _targetWorldPos = worldPos;
            transform.position = worldPos;
            _isMoving = false;
        }
    }
}
