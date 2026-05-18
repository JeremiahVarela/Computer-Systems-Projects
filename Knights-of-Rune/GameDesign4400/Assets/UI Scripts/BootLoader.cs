using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadScenes());
    }

    IEnumerator LoadScenes()
    {
        // Load Systems scene additively
        AsyncOperation loadSystems = SceneManager.LoadSceneAsync("Systems", LoadSceneMode.Additive);
        yield return loadSystems; // Wait until Systems is loaded

        // Load MainMenu scene additively
        AsyncOperation loadMenu = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        yield return loadMenu; // Wait until MainMenu is loaded

        // Set the active scene to MainMenu
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));

        Debug.Log("BootLoader finished loading Systems and MainMenu.");
    }
}
