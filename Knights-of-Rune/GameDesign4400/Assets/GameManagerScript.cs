using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverScreen;   // panel with the buttons

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // set in Inspector

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // If this lives in Systems and Systems is always loaded, you don't need DontDestroyOnLoad.
        // If it's in a normal scene and you want it persistent, uncomment:
        // DontDestroyOnLoad(gameObject);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        Debug.Log("[GameManager] Respawn button pressed.");

        // Unpause and hide the game-over UI
        Time.timeScale = 1f;
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        // Reload current gameplay scene
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void MainMenu()
    {
        Debug.Log("[GameManager] Main Menu button pressed.");

        Time.timeScale = 1f;
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogError("[GameManager] mainMenuSceneName is not set!");
            return;
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("[GameManager] Quit button pressed.");
        Time.timeScale = 1f;

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
