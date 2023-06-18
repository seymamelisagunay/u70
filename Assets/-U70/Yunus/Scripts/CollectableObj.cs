using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Object/Ammo", fileName = "Ammo")]
public class CollectableObj : ScriptableObject
{
    public string ammoName;
    public int ammoAmount;
}
