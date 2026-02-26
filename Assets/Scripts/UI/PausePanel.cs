using UnityEngine;
using UnityEngine.UI;
using StreetEscape.Systems;

namespace StreetEscape.UI
{
    public class PausePanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button menuButton;

        private void OnEnable()
        {
            if (resumeButton != null)
                resumeButton.onClick.AddListener(OnResume);
            if (menuButton != null)
                menuButton.onClick.AddListener(OnMenu);
        }

        private void OnDisable()
        {
            if (resumeButton != null)
                resumeButton.onClick.RemoveListener(OnResume);
            if (menuButton != null)
                menuButton.onClick.RemoveListener(OnMenu);
        }

        private void OnResume()
        {
            GameManager.Instance?.ResumeGame();
        }

        private void OnMenu()
        {
            GameManager.Instance?.ReturnToMenu();
        }
    }
}
