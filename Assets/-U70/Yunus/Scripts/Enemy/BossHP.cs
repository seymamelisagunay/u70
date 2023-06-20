using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BossAI))]
public class BossHP : MonoBehaviour
{
    [Header("HP")]
    public float maxHealth;
    [Range(0f, 1f)]
    public float armour;

    float hp;
    bool dead;
    [HideInInspector] public bool canTakeDmg;

    [Header("UI Objects")]
    public RectTransform hpCanvas;
    public TextMeshProUGUI hpTxt;
    public Image hpImage;


    void Start()
    {
        hp = maxHealth;
        dead = false;
        canTakeDmg = true;

        hpImage.fillAmount = 1;
        hpTxt.text = hp.ToString();
    }
    public void GetDamage(float damage)
    {
        if (canTakeDmg)
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
    }

    void Die()
    {
        if (!dead)
        {
            dead = true;
            UIDisappear();

            GetComponent<BossAI>().Die();
        }
    }
    void UIDisappear()
    {
        hpCanvas.DOScale(hpCanvas.localScale.x * 1.4f, 1f).SetEase(Ease.OutBack);
        hpCanvas.GetComponent<CanvasGroup>().DOFade(0, 1f);
    }
}
