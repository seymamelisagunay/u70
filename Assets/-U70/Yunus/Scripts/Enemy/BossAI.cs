using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("FireBall Attack")]
    public GameObject fireball;
    public float fireballSpeed;
    public Transform muzzleHand;
    public Transform childTransform;                            //animasyondan dolayý
    public float distanceCheckTime;
    public float atkSpeed;

    float range;                                                // bu range player pistol daki range ile ayný yoksa ikisinden birir menzil avantajý kazanmýþ olurdu
    float distCheck;
    float rangeSqr;                                             //range deðiþkeninin karesi optimizasyon için

    bool canAtk;

    Transform pcTransform;                                      //PistolController objesinin transformunu tutar
    Animator anim;

    void Start()
    {
        pcTransform = PlayerHP.ins.transform;
        anim = GetComponentInChildren<Animator>();

        range = PistolController.ins.range;
        rangeSqr = range * range;
        distCheck = distanceCheckTime;

        canAtk = true;
    }
    void Update()
    {
        if (distCheck > 0)
        {
            distCheck = distanceCheckTime;

            DistCheckPlayer();

            Attack();
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
    void Attack()
    {
        if (canAtk)
        {
            canAtk = false;
            Invoke(nameof(ResetAtk), atkSpeed);

            childTransform.SetLocalPositionAndRotation(new Vector3(0, 0.05f, 0), Quaternion.Euler(0, 5, 0));

            anim.SetTrigger("fireball");

            Invoke(nameof(InsFireBall), 0.3f);
        }
    }
    void InsFireBall()
    {
        Vector3 direction = pcTransform.position - muzzleHand.position;

        GameObject a = Instantiate(fireball, muzzleHand.position, Quaternion.identity);
        a.GetComponent<Rigidbody>().velocity = direction * fireballSpeed;
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
