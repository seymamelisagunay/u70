using UnityEngine;

public class GeneralPool : MonoBehaviour
{
    [Header("Flash Obj")]
    public GameObject flash;
    public int flashObjCountInPool;

    public static ObjectPooling flashPool;

    [Header("Blood Obj")]
    public GameObject blood;
    public int bloodObjCountInPool;

    public static ObjectPooling bloodPool;

    [Header("Blood Obj")]
    public GameObject bullet;
    public int bulletObjCountInPool;

    public static ObjectPooling bulletPool;


    void Start()
    {
        flashPool = new ObjectPooling(flash, transform);
        flashPool.HavuzuDoldur(flashObjCountInPool);

        bloodPool = new ObjectPooling(blood, transform);
        bloodPool.HavuzuDoldur(bloodObjCountInPool);

        bulletPool = new ObjectPooling(bullet, transform);
        bulletPool.HavuzuDoldur(bulletObjCountInPool);
    }

    public static void FlashEffect(Vector3 pos, float dieTime)
    {
        GameObject a = flashPool.HavuzdanObjeCek();

        a.transform.position = pos;

        a.GetComponent<Flash>().dieTime = dieTime;
        a.GetComponent<Flash>().pool = flashPool;
    }

    public static void BloodEffect(Vector3 pos, float dieTime)
    {
        GameObject a = bloodPool.HavuzdanObjeCek();

        a.transform.position = pos;

        a.GetComponent<Flash>().dieTime = dieTime;
        a.GetComponent<Flash>().pool = bloodPool;
    }

    public static void BulletAmmo(Vector3 pos, float dieTime)
    {
        GameObject a = bulletPool.HavuzdanObjeCek();

        a.transform.position = pos;

        a.GetComponent<Bullet>().dieTime = dieTime;
        a.GetComponent<Bullet>().pool = bulletPool;
    }
}
