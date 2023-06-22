using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("FireBall Attack")]
    public GameObject fireball;
    public float fireBallDamage;
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

    [Space(10)]
    public ParticleSystem callShield;
    public ParticleSystem callCircle;
    public Collider colliderr;

    BossHP bossHP;


    void Start()
    {
        pcTransform = PlayerHP.ins.transform;
        anim = GetComponentInChildren<Animator>();
        bossHP = GetComponent<BossHP>();

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
        a.GetComponent<BossFireBall>().damage = fireBallDamage;
    }
    void CallSkeleton()
    {
        canAtk = false;
        Invoke(nameof(ResetAtk), atkSpeedSkeletonCall);

        CanBossTakeDmg(false);
        anim.SetTrigger("skeleton");

        Invoke(nameof(InsSkeleton), 4f);
    }
    void InsSkeleton()
    {
        childTransform.SetLocalPositionAndRotation(new Vector3(0, 0.05f, 0), Quaternion.Euler(0, 5, 0));

        for (int i = 0; i < calledEnemyCount; i++)
        {
            Vector3 spawnPos = new(transform.position.x + Random.Range(-callRange, callRange), transform.position.y - 2, transform.position.z + Random.Range(-callRange, callRange));

            Instantiate(callCircle, spawnPos + new Vector3(0, 2.1f, 0), Quaternion.Euler(90, 0, 0));
            GameObject skeleton = Instantiate(enemy, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));

            skeleton.GetComponent<NavMeshAgent>().enabled = false; 
            skeleton.GetComponent<EnemyNavMesh>().enabled = false;
            skeleton.GetComponent<EnemyHP>().enabled = false;

            skeleton.transform.DOMoveY(skeleton.transform.position.y + 2, 1.5f);

            StartCoroutine(StartSkeletonSystem(1.5f, skeleton));
            callShield.Stop();
        }
    }
    IEnumerator StartSkeletonSystem(float waitTime, GameObject skeleton)
    {
        yield return new WaitForSeconds(waitTime);
        CanBossTakeDmg(true);

        skeleton.GetComponent<NavMeshAgent>().enabled = true;
        skeleton.GetComponent<EnemyNavMesh>().enabled = true;
        skeleton.GetComponent<EnemyHP>().enabled = true;

        skeleton.GetComponent<EnemyNavMesh>().FollowPlayer();
    }
    void CanBossTakeDmg(bool truee)
    {
        if (truee)
        {
            callShield.Stop();
            bossHP.canTakeDmg = true;
            colliderr.enabled = true;
        }
        else
        {
            callShield.Play();
            bossHP.canTakeDmg = false;
            colliderr.enabled = false;
        }
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
