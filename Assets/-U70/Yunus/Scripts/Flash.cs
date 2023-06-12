using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public ObjectPooling pool;
    public float dieTime;


    void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(SendBulletToPool());
    }

    IEnumerator SendBulletToPool()
    {
        yield return new WaitForSeconds(dieTime);
        pool.HavuzaObjeEkle(gameObject);
    }
}
