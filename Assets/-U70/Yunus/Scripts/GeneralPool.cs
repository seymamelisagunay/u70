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


    void Start()
    {
        flashPool = new ObjectPooling(flash, transform);
        flashPool.HavuzuDoldur(flashObjCountInPool);

        bloodPool = new ObjectPooling(blood, transform);
        bloodPool.HavuzuDoldur(bloodObjCountInPool);
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
}
