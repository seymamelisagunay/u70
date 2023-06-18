using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public CollectableObj bullet;
    public int bulletIncAmount;

    public CollectableObj shipBall;
    public int shipBallIncAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            bullet.ammoAmount += bulletIncAmount;
            UptBulletTxt();
        }
        if (other.CompareTag("ShipBall"))
        {
            Destroy(other.gameObject);
            shipBall.ammoAmount += shipBallIncAmount;
            UptShipBallTxt();
        }
    }

    void UptBulletTxt()
    {

    }
    void UptShipBallTxt()
    {

    }
}
