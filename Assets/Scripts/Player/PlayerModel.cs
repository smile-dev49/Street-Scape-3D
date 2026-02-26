using UnityEngine;

namespace StreetEscape.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Animator animator;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsHit = Animator.StringToHash("IsHit");

        public void SetRunning(bool running)
        {
            if (animator != null)
                animator.SetBool(IsRunning, running);
        }

        public void TriggerHit()
        {
            if (animator != null)
                animator.SetTrigger(IsHit);
        }

        public void SetMaterial(Material mat)
        {
            if (meshRenderer != null && mat != null)
                meshRenderer.material = mat;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
