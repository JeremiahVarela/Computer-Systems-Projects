using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Damage + knockback
    public void TakeDamage(int dmg, Vector2 knockbackDir, float knockbackForce)
    {
        health -= dmg;
        Debug.Log(name + " took " + dmg + " damage. Health: " + health);

        // Apply knockback if Rigidbody2D exists
        if (rb != null)
        {
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }

        // Kill enemy if health reaches zero
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death effects here if needed
        Destroy(gameObject);
    }
}


