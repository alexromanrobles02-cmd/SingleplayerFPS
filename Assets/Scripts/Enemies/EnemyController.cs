using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int health = 100;

    void Start()
    {
        player = PlayerController.Instance.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        if (player != null)
        {
            navMeshAgent.SetDestination(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

