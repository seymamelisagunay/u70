using DG.Tweening;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [Header("Attack System")]
    public float attackDamage;
    public float attackSpeed;
    public float atkLookTargetTime;

    NavMeshAgent agent;
    Transform target;
    Animator animator;

    bool follow, canAtk;
    [HideInInspector] public bool canGiveDmg;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<FirstPersonController>().transform;

        follow = false;
        canAtk = true;
        canGiveDmg = false;

        agent.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            follow = true;
            agent.isStopped = false;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            follow = false;
            agent.isStopped = true;
        }
    }

    private void FixedUpdate()
    {
        if (follow)
        {
            agent.destination = target.position;
        }
        if (IsAgentReachTarget() && !agent.isStopped)
        {
            Attack();
        }


        Vector2 flatVelocity = new Vector2(agent.velocity.x, agent.velocity.z);
        float speed = flatVelocity.magnitude;

        animator.SetFloat("speedh",speed);
    }

    public void Die()
    {
        agent.isStopped = true;
        animator.SetTrigger("Fall1");

        transform.DOMoveY(transform.position.y - 1, 3).SetDelay(2);

        //can bar kýsmý kapansýn

        Destroy(gameObject, 5);
    }

    bool IsAgentReachTarget()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance) return true;

        return false;
    }

    void Attack()
    {
        if (canAtk)
        {
            canGiveDmg = true;
            canAtk = false;

            animator.SetTrigger("Attack1h1");
            transform.DOLookAt(target.position + new Vector3(0.3f, 0, 0), atkLookTargetTime);

            Invoke(nameof(ResetAtk), attackSpeed);
        }
    }
    void ResetAtk()
    {
        canAtk = true;
    }
}
