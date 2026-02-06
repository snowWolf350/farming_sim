using UnityEngine;

[CreateAssetMenu()]
public class PlantSO : ScriptableObject
{
    public string plantName;
    public GameObject plantPrefab;
    public GameObject seedVisual;
    public GameObject fullyDevelopedVisual;
    public GameObject halfDevelopedVisual;
    public Sprite plantIcon;
    public Sprite SeedIcon;
}
