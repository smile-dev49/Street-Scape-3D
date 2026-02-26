using UnityEngine;

namespace StreetEscape.Lanes
{
    public class RoadSegment : MonoBehaviour
    {
        [SerializeField] private float length = 20f;
        [SerializeField] private float scrollSpeed = 5f;

        public float Length => length;

        public void Move(float delta)
        {
            transform.position -= transform.forward * (scrollSpeed * delta);
        }

        public void ResetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
