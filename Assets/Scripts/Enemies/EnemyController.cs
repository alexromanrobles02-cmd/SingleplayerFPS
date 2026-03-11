using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Normal, Fast, Tank }
    
    [Header("Type")]
    [Tooltip("Selecciona el tipo de enemigo (Afecta vida, velocidad y daño)")]
    public EnemyType enemyType = EnemyType.Normal;

    [HideInInspector] public int damageToPlayer = 25;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int health = 75;
    private Animator animator;

    private float detectionDistance = 15f;
    private bool isDead = false;

    void Start()
    {
        player = PlayerController.Instance.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetupEnemyType();

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy();
        }
    }

    private void SetupEnemyType()
    {
        switch (enemyType)
        {
            case EnemyType.Normal:
                health = 75;
                damageToPlayer = 25;
                if (navMeshAgent != null) navMeshAgent.speed = 3.5f;
                detectionDistance = 15f;
                // Escala normal
                transform.localScale = Vector3.one;
                break;
            case EnemyType.Fast:
                health = 30;
                damageToPlayer = 10;
                if (navMeshAgent != null) navMeshAgent.speed = 6.0f;
                detectionDistance = 25f; // Detecta desde más lejos
                // Un poco más pequeño
                transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                break;
            case EnemyType.Tank:
                health = 200;
                damageToPlayer = 50;
                if (navMeshAgent != null) navMeshAgent.speed = 1.5f;
                detectionDistance = 10f; // Detecta de más cerca
                // Un poco más grande
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                break;
        }
    }

    public void TakeDamage(int damage, Vector3 hitPosition, Quaternion hitRotation)
    {
        if (isDead) return;

        BloodParticlesManager.Instance.PlayParticlesAt(hitPosition, hitRotation);

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isDead) return;

        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < detectionDistance)
            {
                navMeshAgent.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
        }

        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private void Die()
    {
        navMeshAgent.SetDestination(transform.position);
        isDead = true;
        animator.Play("Die");

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy();
        }

        Destroy(gameObject, 5f);
    }
}



