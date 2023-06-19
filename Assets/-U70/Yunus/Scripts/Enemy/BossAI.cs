using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("FireBall Attack")]
    public GameObject fireball;
    public float fireballSpeed;
    public Transform muzzleHand;
    public Transform childTransform;                            //animasyondan dolayý
    public float distanceCheckTime;
    public float atkSpeedFireBall;

    float range;                                                // bu range player pistol daki range ile ayný yoksa ikisinden birir menzil avantajý kazanmýþ olurdu
    float distCheck;
    float rangeSqr;                                             //range deðiþkeninin karesi optimizasyon için
    int attackCount;

    bool canAtk;

    Transform pcTransform;                                      //PistolController objesinin transformunu tutar
    Animator anim;

    [Header("Skeleton Attack")]
    public GameObject enemy;
    public int calledEnemyCount;
    public float callRange;
    public int whichTimeFireBall;                               // boss kaç saldýrýda bir iskelet çýkartacak
    public float atkSpeedSkeletonCall;



    void Start()
    {
        pcTransform = PlayerHP.ins.transform;
        anim = GetComponentInChildren<Animator>();

        range = PistolController.ins.range + 1;
        rangeSqr = range * range;
        distCheck = distanceCheckTime;

        canAtk = true;
        attackCount = 0;
    }
    void Update()
    {
        if (distCheck > 0)
        {
            distCheck = distanceCheckTime;

            DistCheckPlayer();

            if (canAtk)
            {
                if (attackCount < whichTimeFireBall)
                {
                    attackCount++;
                    FireBallAttack();
                }
                else
                {
                    attackCount = 0;
                    CallSkeleton();
                }
            }
            
        }
        else
            distCheck -= Time.deltaTime;
    }

    void DistCheckPlayer()
    {
        if (Vector3.SqrMagnitude(pcTransform.position - transform.position) < rangeSqr)     //player silahýnýn range deðerinin içerisinde 
        {
            Vector3 horizontalTarget = new(pcTransform.position.x, transform.position.y, pcTransform.position.z);   //bir objeye sadece yatayda bakma kody
            transform.LookAt(horizontalTarget);
        }
    }
    void FireBallAttack()
    {
        canAtk = false;
        Invoke(nameof(ResetAtk), atkSpeedFireBall);

        anim.SetTrigger("fireball");

        Invoke(nameof(InsFireBall), 0.3f);
    }
    void InsFireBall()
    {
        Vector3 direction = pcTransform.position - muzzleHand.position + new Vector3(0, 1f, 0);

        GameObject a = Instantiate(fireball, muzzleHand.position, Quaternion.identity);
        a.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;
    }
    void CallSkeleton()
    {
        canAtk = false;
        Invoke(nameof(ResetAtk), atkSpeedSkeletonCall);

        anim.SetTrigger("skeleton");

        Invoke(nameof(InsSkeleton), 4f);
    }
    void InsSkeleton()
    {
        childTransform.SetLocalPositionAndRotation(new Vector3(0, 0.05f, 0), Quaternion.Euler(0, 5, 0));

        for (int i = 0; i < calledEnemyCount; i++)
        {
            GameObject a = Instantiate(enemy, transform.position, Quaternion.identity);
            a.transform.position = new Vector3(transform.position.x + Random.Range(-callRange, callRange), transform.position.y - 2, transform.position.z + Random.Range(-callRange, callRange));

            a.GetComponent<NavMeshAgent>().enabled = false; 
            a.GetComponent<EnemyNavMesh>().enabled = false; 

            a.transform.DOMoveY(a.transform.position.y + 2, 1.5f);


            Invoke(nameof(StartSkeletonSystem), 1.5f);
        }
    }
    void StartSkeletonSystem()
    { 
        //a.GetComponent<NavMeshAgent>().enabled = false;
        //a.GetComponent<EnemyNavMesh>().enabled = false;

        //doðduklarýnda saldýrsýnlar
    }


    public void Die()
    {
        Destroy(gameObject, 0.1f);
    }

    void ResetAtk()
    {
        canAtk = true;
    }
}
