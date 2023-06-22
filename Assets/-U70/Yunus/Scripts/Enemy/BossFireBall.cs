using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    public ParticleSystem fireBallVFX;
    public ParticleSystem fireBallExpVFX;
    
    public float damage;
    public float dieTime;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.name == "Capsule")
        {
            other.GetComponentInParent<PlayerHP>().GetDamage(damage);

            StopFireball();
        }

        if (other.CompareTag("Untagged") && other.CompareTag("Ground"))
        {
            StopFireball();
        }
    }

    void StopFireball()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        fireBallVFX.Stop();
        fireBallExpVFX.Play();
        Destroy(gameObject, dieTime);
    }
}
