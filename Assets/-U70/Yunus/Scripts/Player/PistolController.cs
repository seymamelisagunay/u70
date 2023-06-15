using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PistolController : MonoBehaviour
{
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
    public float bulletDamage;
    public float maxAmmo;
    public TextMeshProUGUI ammoTxt;

    Transform muzzlePos;
    float ammo;

    [Space(10)]
    public Camera fpsCam;                                   //cameradan ileri ray atacaz ve deydiði yere mermi ateþleyecez
    public float range;


    void Start()
    {
        pistolObj = GetComponent<Transform>();
        muzzlePos = muzzleFlashPS.transform;


        canAtk = true;
        isFrontWall = false;

        ammo = maxAmmo;
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
        if (canAtk && !isFrontWall && ammo > 0)
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
        if (canAtk && !isFrontWall && ammo < maxAmmo)
        {
            ammo = maxAmmo;
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
                hit.transform.GetComponentInParent<EnemyHP>().GetDamage(bulletDamage);
            }
            else if (hit.transform.CompareTag("EnemySkeleton"))
            {
                GeneralPool.FlashEffect(hit.point, 1);
                hit.transform.GetComponentInParent<EnemyHP>().GetDamage(bulletDamage);
            }
            else
            {
                GeneralPool.FlashEffect(hit.point, 1);
            }
        }
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
