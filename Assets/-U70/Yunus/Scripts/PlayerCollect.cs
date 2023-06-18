using DG.Tweening;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public static PlayerCollect ins;

    public TextMeshProUGUI totalAmmoTxt;
    public TextMeshProUGUI collectTxt;

    [Space(10)]

    public CollectableObj bullet;
    public int bulletIncAmount;

    public CollectableObj shipBall;
    public int shipBallIncAmount;

    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        UptAmmo(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);

            UptCollectTxt(bulletIncAmount, " Bullet");

            UptAmmo(bulletIncAmount);
        }
        if (other.CompareTag("ShipBall"))
        {
            Destroy(other.gameObject);
            
            shipBall.ammoAmount += shipBallIncAmount;

            UptCollectTxt(shipBallIncAmount, " Cannon Ball");
        }
    }

    void UptCollectTxt(int amount, string kind)
    {
        collectTxt.text = "+ " + amount.ToString() + kind;

        collectTxt.GetComponent<RectTransform>().DOKill();
        collectTxt.GetComponent<CanvasGroup>().DOKill();

        collectTxt.GetComponent<RectTransform>().DOScale(1.1f, 0.2f);
        collectTxt.GetComponent<RectTransform>().DOScale(1f, 0.3f).SetDelay(0.2f);

        collectTxt.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
        collectTxt.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetDelay(0.5f);
    }

    public void UptAmmo(int incOrReduAmmo)         //increase or reduce ammo 
    {
        bullet.ammoAmount += incOrReduAmmo;

        if (bullet.ammoAmount > PistolController.ins.maxPocketAmmo)
            bullet.ammoAmount = PistolController.ins.maxPocketAmmo;

        totalAmmoTxt.text = bullet.ammoAmount.ToString() + " / " + PistolController.ins.maxPocketAmmo.ToString();
    }
}
