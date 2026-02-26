using UnityEngine;

namespace StreetEscape.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject shopPanel;

        private void Awake()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            SetActive(mainMenuPanel, true);
            SetActive(hudPanel, false);
            SetActive(pausePanel, false);
            SetActive(gameOverPanel, false);
            SetActive(shopPanel, false);
        }

        public void ShowHUD()
        {
            SetActive(mainMenuPanel, false);
            SetActive(hudPanel, true);
            SetActive(pausePanel, false);
            SetActive(gameOverPanel, false);
        }

        public void ShowPausePanel()
        {
            SetActive(pausePanel, true);
        }

        public void HidePausePanel()
        {
            SetActive(pausePanel, false);
        }

        public void ShowGameOverPanel()
        {
            SetActive(gameOverPanel, true);
        }

        public void ShowShopPanel()
        {
            SetActive(shopPanel, true);
        }

        public void HideShopPanel()
        {
            SetActive(shopPanel, false);
        }

        private static void SetActive(GameObject obj, bool active)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
