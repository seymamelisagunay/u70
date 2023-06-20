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

    float defaultSpeed;

    Collider bodyColl;
    NavMeshAgent agent;
    Animator animator;

    Transform target;
    Transform pistolAsTarget;                       //pistol'u target olarak atad�k ��nk� iskeletler hafiften sola do�ru vuruyor
    public Transform skeletonChild;                        //bu de�i�ken ile skeleton chil objesini'nin transform.rotation de�i�kenini s�rekli (0,0,0) de�erinde tutmam�z laz�m yoksa d�n�yor

    bool follow, canAtk, isAlive;
    [HideInInspector] public bool canGiveDmg;


    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<FirstPersonController>().transform;
        pistolAsTarget = FindObjectOfType<PistolController>().transform;
        bodyColl = GetComponent<CapsuleCollider>();

        follow = false;
        canAtk = true;
        canGiveDmg = false;
        isAlive = true;

        agent.isStopped = true;

        defaultSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                FollowPlayer();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                StopFollow();
            }
        }
    }
    public void FollowPlayer()
    {
        follow = true;
        agent.isStopped = false;
    }
    public void StopFollow()
    {
        follow = false;
        agent.isStopped = true;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            if (follow && isAlive)
            {
                agent.destination = target.position;
                agent.speed = defaultSpeed;
            }
            if (IsAgentReachTarget() && !agent.isStopped)
            {
                Attack();
            }

            Vector2 flatVelocity = new Vector2(agent.velocity.x, agent.velocity.z);
            float speed = flatVelocity.magnitude;

            animator.SetFloat("speedh", speed);
        }
    }

    public void Die()
    {
        isAlive = false;
        agent.isStopped = true;

        bodyColl.enabled = false;               //�ld�kten sonra di�er karakterler i�inden ge�ebilsin diye
        Invoke(nameof(Awd), 0.1f);

        //animator.SetTrigger("Fall1");         //�lme animasyonu - gerek yok ragdoll ile hallettik

        transform.DOMoveY(transform.position.y - 5, 1).SetDelay(2.5f);

        Destroy(gameObject, 3.5f);
    }
    void Awd()
    {
        animator.enabled = false;       //�ld�kten sonra ragdoll olsun diye
    }


    bool IsAgentReachTarget()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance) return true;

        return false;
    }

    void Attack()
    {
        if (canAtk && PlayerHP.ins.isAlive)
        {
            skeletonChild.localEulerAngles = new Vector3(0, 0, 0);
            agent.speed = defaultSpeed * 0.5f;

            follow = false;
            canGiveDmg = true;
            canAtk = false;

            animator.SetTrigger("Attack1h1");

            Vector3 horizontalTarget = new(pistolAsTarget.position.x, transform.position.y, pistolAsTarget.position.z);
            transform.DOLookAt(horizontalTarget, atkLookTargetTime).SetDelay(0.2f);

            Invoke(nameof(ResetAtk), attackSpeed);
        }
    }
    void ResetAtk()
    {
        follow = true;
        canAtk = true;
    }
}
