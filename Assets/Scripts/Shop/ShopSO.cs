using UnityEngine;

[CreateAssetMenu()]
public class ShopSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public GameObject itemGameobject;
    public PlantSO plantSO;
    public Vector3 spawnPos;
    public float itemPrice;
}
