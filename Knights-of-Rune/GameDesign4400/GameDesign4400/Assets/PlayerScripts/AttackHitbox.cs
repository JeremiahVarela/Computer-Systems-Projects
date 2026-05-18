using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackHitbox : MonoBehaviour
{
    // List of enemies currently inside the circle hitbox
    public List<Enemy> enemiesInRange = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }
}


