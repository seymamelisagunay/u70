using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProducer : MonoBehaviour
{
    public GameObject enemy;
    public float timer;

    void Start()
    {
        InvokeRepeating(nameof(ProduceEnemy), 0, timer);
    }
    void ProduceEnemy()
    {
        GameObject a = Instantiate(enemy, transform.position, Quaternion.identity, transform);

        a.transform.position = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5));
    }
}
