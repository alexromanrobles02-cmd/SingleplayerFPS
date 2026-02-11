using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int health = 100;
    private Animator animator;

    private float detectionDistance = 5f;
    private bool isDead = false;

    void Start()
    {
        player = PlayerController.Instance.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Vector3 hitPosition, Quaternion hitRotation)
    {
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
        Destroy(gameObject, 5f);
    }
}

