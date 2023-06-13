using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("HP")]
    public float maxHealth;
    [Range(0f, 1f)]
    public float armour;

    float hp;

    [Header("Objects")]
    public TextMeshProUGUI hpTxt;
    public Image hpImage;


    void Start()
    {
        hp = maxHealth;

        hpImage.fillAmount = 1;
        hpTxt.text = hp.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && other.GetComponent<EnemyWeapon>().enemyHolder.canGiveDmg)
        {
            other.GetComponent<EnemyWeapon>().enemyHolder.canGiveDmg = false;
            GetDamage(other.GetComponent<EnemyWeapon>().enemyHolder.attackDamage);
        }
    }
    public void GetDamage(float damage)
    {
        hp -= damage - damage * armour;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        hpImage.fillAmount = hp / maxHealth;
        hpTxt.text = hp.ToString();
    }

    void Die()
    {
        //Player ölme sistemleri
    }
}
