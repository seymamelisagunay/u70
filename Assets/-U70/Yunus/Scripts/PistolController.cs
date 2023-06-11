using UnityEngine;

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
    public float bulletSpeed;
    Transform muzzlePos;

    void Start()
    {
        pistolObj = GetComponent<Transform>();
        muzzlePos = muzzleFlashPS.transform;


        canAtk = true;
        isFrontWall = false;
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
        if (canAtk && !isFrontWall)
        {
            pistolAnim.SetTrigger("pistolShoot");

            sparklingPS.Play();
            Invoke(nameof(SmokeDelay), 0.15f);

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), attackSpeed);

            Invoke(nameof(InstantiateBullet), 0.12f);
        }
    }
    public void ReloadPistol()
    {
        if (canAtk && !isFrontWall)
        {
            pistolAnim.SetTrigger("pistolReload");

            canAtk = false;
            Invoke(nameof(ResetAtkSpeed), reloadTime);
        }
    }
    void InstantiateBullet()
    {
        GameObject a = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        a.GetComponent<Rigidbody>().velocity = muzzlePos.right * bulletSpeed;
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
