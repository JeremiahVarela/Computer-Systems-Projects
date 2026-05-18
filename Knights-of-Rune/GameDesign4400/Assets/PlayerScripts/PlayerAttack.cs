using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRadius = 1.5f;        // size of invisible circle
    public int damage = 10;
    public float attackCooldown = 0.3f;

    [Header("Knockback")]
    public float knockbackForce = 5f;

    [Header("Layers")]
    public LayerMask enemyLayer;             // set this in inspector

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            DoAttack();
        }
    }

    void DoAttack()
    {
        // Detect all enemies inside the invisible circle
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 dir = (enemy.transform.position - transform.position).normalized;

                enemy.TakeDamage(damage, dir, knockbackForce);
            }
        }
    }

    // Draw circle in Scene view (not game view)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

