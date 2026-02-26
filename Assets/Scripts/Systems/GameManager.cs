using UnityEngine;
using StreetEscape.UI;

namespace StreetEscape.Systems
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private UIManager uiManager;

        private GameState _state = GameState.MainMenu;

        public GameState State => _state;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame()
        {
            _state = GameState.Playing;
            Time.timeScale = 1f;
            uiManager?.ShowHUD();
        }

        public void PauseGame()
        {
            _state = GameState.Paused;
            Time.timeScale = 0f;
            uiManager?.ShowPausePanel();
        }

        public void ResumeGame()
        {
            _state = GameState.Playing;
            Time.timeScale = 1f;
            uiManager?.HidePausePanel();
        }

        public void GameOver()
        {
            _state = GameState.GameOver;
            Time.timeScale = 0f;
            uiManager?.ShowGameOverPanel();
        }

        public void ReturnToMenu()
        {
            _state = GameState.MainMenu;
            Time.timeScale = 1f;
            uiManager?.ShowMainMenu();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
