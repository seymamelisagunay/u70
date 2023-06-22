using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP ins;

    [Header("HP")]
    public float maxHealth;
    [Range(0f, 1f)]
    public float armour;

    float hp;
    [HideInInspector] public bool isAlive;

    [Header("Objects")]
    public TextMeshProUGUI hpTxt;
    public Image hpImage;


    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        hp = maxHealth;

        isAlive = true;

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
        isAlive = false;
        GetComponent<FirstPersonController>().isAlive = false;

        UIController.ins.YouDied();
    }
}
