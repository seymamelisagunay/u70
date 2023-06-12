using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
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
    public void GetDamage(float damage)
    {
        hp -= damage - damage * armour;

        if (hp <= 0)
        {
            hp = 0;
            Die();
        }

        hpImage.fillAmount = hp/maxHealth;
        hpTxt.text = hp.ToString();
    }

    void Die()
    {
        Destroy(gameObject);
        //�lme animasyonunu oynat
        //hareketi s�f�rla
        //2 sn sonra d��manlar a�a�� do�ru insinler 
        //yerde g�z�kmedikleri zaman destroy olsunlar
    }
}
