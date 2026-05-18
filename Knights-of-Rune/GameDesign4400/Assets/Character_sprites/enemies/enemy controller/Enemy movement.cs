using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyChaseAI2D : MonoBehaviour
{
    private PlayerHealth PlayerHealth;      


    [Header("Targeting")]
    public Transform target;
    [Tooltip("If left empty, will automatically find the Player tag.")]
    public string targetTag = "Player";
    public float detectionRange = 10f;
    public float attackRange = 1.5f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public bool flipXToFace = true;

    [Header("Attack")]
    public float attackCooldown = 1.2f;
    public int damage = 10;

    [Header("Animation")]
    public Sprite[] moveFrames;
    public Sprite[] attackFrames;
    public float moveFPS = 8f;
    public float attackFPS = 10f;
    public bool loopMove = true;

    Rigidbody2D rb;
    SpriteRenderer sr;
    float attackTimer;
    float animClock;
    int frameIndex;
    enum State { Idle, Chase, Attack }
    State state = State.Idle;
    Vector2 desiredVel;
    float lastFacingX = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        if (!target)
        {
            GameObject p = GameObject.FindGameObjectWithTag(targetTag);
            if (p) target = p.transform;
        }
    }

    void Update()
    {
        if (!target) return;
        float dist = Vector2.Distance(transform.position, target.position);
        attackTimer -= Time.deltaTime;

        if (dist <= attackRange)
        {
            state = State.Attack;
        }
        else if (dist <= detectionRange)
        {
            state = State.Chase;
        }
        else
        {
            state = State.Idle;
        }

        switch (state)
        {
            case State.Chase:
                desiredVel = (target.position - transform.position).normalized * moveSpeed;
                break;
            case State.Idle:
                desiredVel = Vector2.zero;
                break;
            case State.Attack:
                desiredVel = Vector2.zero;
                if (attackTimer <= 0f)
                {
                    attackTimer = attackCooldown;
                    StartCoroutine(AttackRoutine());
                }
                break;
        }

        // Sprite facing direction
        if (flipXToFace && Mathf.Abs(rb.velocity.x) > 0.05f)
        {
            lastFacingX = Mathf.Sign(rb.velocity.x);
            sr.flipX = (lastFacingX < 0f);
        }

        Animate();
    }

    void FixedUpdate()
    {
        // Smooth velocity change
        Vector2 v = rb.velocity;
        Vector2 diff = desiredVel - v;
        float rate = (desiredVel.sqrMagnitude > 0.01f) ? acceleration : deceleration;
        Vector2 step = Vector2.ClampMagnitude(diff, rate * Time.fixedDeltaTime);
        rb.velocity = v + step;
    }

    void Animate()
    {
        Sprite[] frames = null;
        float fps = 0f;
        bool loop = true;

        if (state == State.Attack && attackFrames != null && attackFrames.Length > 0)
        {
            frames = attackFrames;
            fps = attackFPS;
            loop = false;
        }
        else if (state == State.Chase && moveFrames != null && moveFrames.Length > 0)
        {
            frames = moveFrames;
            fps = moveFPS;
            loop = loopMove;
        }
        else
        {
            // idle = first frame of move animation
            if (moveFrames != null && moveFrames.Length > 0)
                sr.sprite = moveFrames[0];
            return;
        }

        animClock += Time.deltaTime * fps;
        int i = loop ? (int)animClock % frames.Length : Mathf.Min((int)animClock, frames.Length - 1);
        sr.sprite = frames[i];
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        animClock = 0f;  // restart attack animation
        if (attackFrames != null && attackFrames.Length > 0)
        {
            for (int i = 0; i < attackFrames.Length; i++)
            {
                sr.sprite = attackFrames[i];
                yield return new WaitForSeconds(1f / attackFPS);
            }
        }

        // Damage (very simple trigger example)
        if (target && Vector2.Distance(transform.position, target.position) <= attackRange + 0.2f)
        {
            // Debug.Log($"{name} attacked {target.name} for {damage} damage!");
            // TODO: add player health script and call TakeDamage(damage);
            PlayerHealth = target.GetComponent<PlayerHealth>();
            PlayerHealth.TakeDamage(damage);


        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
