using UnityEngine;
using UnityEngine.SceneManagement;

public class InventorySceneVisibility : MonoBehaviour
{
    [Header("Assign the Canvas you want to hide/show")]
    [SerializeField] Canvas uiCanvas;  

    [Header("Name of your gameplay scene (case sensitive)")]
    [SerializeField] string gameplaySceneName = "MainScene";

    void OnEnable()  => SceneManager.activeSceneChanged += OnSceneChanged;
    void OnDisable() => SceneManager.activeSceneChanged -= OnSceneChanged;

    void Start() => UpdateVisibility(SceneManager.GetActiveScene().name);

    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        UpdateVisibility(newScene.name);
    }

    void UpdateVisibility(string sceneName)
    {
        bool shouldShow = sceneName == gameplaySceneName;
        if (uiCanvas)
        {
            // Hide or show the entire canvas
            uiCanvas.enabled = shouldShow;
        }
    }
}

