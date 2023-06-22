using TMPro;
using UnityEngine;

public class PistolController : MonoBehaviour
{
    public static PistolController ins;

    [Header("Pistol Follow")]
    Transform pistolObj;
    public Transform followPivot;
    public float followSpeed;


    [Header("Pistol Anim")]
    public Animator pistolAnim;
    public float attackSpeed, reloadTime;

    bool canAtk;

    [Header("Is Pistol In Ground")]
    public Transform wallTestRayPos;
    public float rayRange;
    public LayerMask ground;

    bool isFrontWall;


    [Header("Attack VFX")]
    public ParticleSystem muzzleFlashPS;
    public ParticleSystem sparklingPS;

    [Header("Attack System")]
    public GameObject bulletPrefab;
    public TextMeshProUGUI ammoTxt;
    public CollectableObj bullet;
    public float bulletDamage;
    public int maxPistolAmmo;
    public int maxPocketAmmo;

    Transform muzzlePos;
    int ammo;

    [Space(10)]
    public Camera fpsCam;                                   //cameradan ileri ray atacaz ve deydiði yere mermi ateþleyecez
    public float range;


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        pistolObj = GetComponent<Transform>();
        muzzlePos = muzzleFlashPS.transform;


        canAtk = true;
        isFrontWall = false;

        ammo = maxPistolAmmo;
        ammoTxt.text = ammo.ToString();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadPistol();
        }


        isFrontWall = Physics.Raycast(wallTestRayPos.position, followPivot.forward, rayRange, ground);
        Debug.DrawRay(wallTestRayPos.position, followPivot.forward * rayRange, Color.red, 0.1f);


        if (!isFrontWall)        //pistol duvarýn içinde deðil
        {
            pistolObj.position = Vector3.Lerp(pistolObj.position, followPivot.position, Time.deltaTime * followSpeed);
            pistolObj.rotation = Quaternion.Lerp(pistolObj.rotation, followPivot.rotation, Time.deltaTime * followSpeed);
        }
        else
        {
            pistolObj.position = Vector3.Lerp(pistolObj.position, followPivot.position, Time.deltaTime * followSpeed);
            pistolObj.rotation = Quaternion.Lerp(pistolObj.rotation,
                followPivot.rotation * Quaternion.Euler(followPivot.rotation.x + 5, followPivot.rotation.y - 80, followPivot.rotation.z),
                Time.deltaTime * followSpeed * 0.3f);
        }
    }

    public void Shoot() //Daha sonradan mobil için ui'a bir buton eklenecek
    {
        if (canAtk && !isFrontWall && ammo > 0 && PlayerHP.ins.isAlive)
        {
            pistolAnim.SetTrigger("pistolShoot");

            sparklingPS.Play();
            Invoke(nameof(SmokeDelay), 0.15f);

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), attackSpeed);

            Invoke(nameof(InstantiateBullet), 0.15f);

            ammo--;
            ammoTxt.text = ammo.ToString();

            if (ammo <= 0)
            {
                Invoke(nameof(ReloadPistol), attackSpeed + 0.05f);
            }
        }
        else if (ammo <= 0)
        {
            ReloadPistol();
        }
    }
    public void ReloadPistol()
    {
        if (canAtk && !isFrontWall && ammo < maxPistolAmmo && ThereIsAmmo() && PlayerHP.ins.isAlive)
        {
            int reloadAmmo = ReloadAmmo();

            PlayerCollect.ins.UptAmmo(ammo - reloadAmmo);
            ammo = reloadAmmo;
            ammoTxt.text = ammo.ToString();

            pistolAnim.SetTrigger("pistolReload");

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), reloadTime);
        }
    }
    void InstantiateBullet()
    {
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))       //bu ray bir þeye çarparsa true döndürür
        {
            if (hit.transform.CompareTag("EnemyBlood"))
            {
                GeneralPool.BloodEffect(hit.point, 1);
                hit.transform.GetComponentInParent<BossHP>().GetDamage(bulletDamage);

                UIController.ins.ShowHitUI();
            }
            else if (hit.transform.CompareTag("EnemySkeleton"))
            {
                GeneralPool.FlashEffect(hit.point, 1);
                hit.transform.GetComponentInParent<EnemyHP>().GetDamage(bulletDamage);

                UIController.ins.ShowHitUI();
            }
            else
            {
                GeneralPool.FlashEffect(hit.point, 1);
            }
        }
    }
    bool ThereIsAmmo()
    {
        return bullet.ammoAmount > 0;
    }
    int ReloadAmmo()
    {
        if (bullet.ammoAmount + ammo >= maxPistolAmmo)
            return maxPistolAmmo;
        else if (ammo == 0)
            return bullet.ammoAmount;
        else
            return bullet.ammoAmount + ammo;
    }



    void ResetAtkSpeed()
    {
        canAtk = true;
    }
    void SmokeDelay()
    {
        muzzleFlashPS.Play();
    }
}
