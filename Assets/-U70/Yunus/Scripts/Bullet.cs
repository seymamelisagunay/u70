using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ObjectPooling pool;
    public float dieTime;


    void OnEnable()
    {
        StartCoroutine(SendBulletToPool());
    }

    IEnumerator SendBulletToPool()
    {
        yield return new WaitForSeconds(dieTime);
        pool.HavuzaObjeEkle(gameObject);
    }
}
