using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (HealthService.Instance != null)
        {
            HealthService.Instance.SetMax(maxHealth);
            HealthService.Instance.Set(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthService not found. Ensure Systems scene is loaded before gameplay.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        Debug.Log($"[Player] Damage -> {currentHealth}");
        HealthService.Instance?.Set(currentHealth);

        if (currentHealth == 0)
        {
            // Show game over / respawn screen instead of instantly changing scene
            if (GameManagerScript.Instance != null)
            {
                GameManagerScript.Instance.ShowGameOverScreen();
            }
            else
            {
                Debug.LogWarning("GameManagerScript.Instance is null - make sure a GameManager exists in the scene.");
            }
        }
    }
}