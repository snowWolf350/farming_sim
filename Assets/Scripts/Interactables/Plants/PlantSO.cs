using UnityEngine;

[CreateAssetMenu()]
public class PlantSO : ScriptableObject
{
    public string plantName;
    [Header("visuals")]
    public GameObject plantPrefab;
    public GameObject seedVisual;
    public GameObject fullyDevelopedVisual;
    public GameObject halfDevelopedVisual;
    public GameObject fruitVisual;
    [Header("icons")]
    public Sprite plantIcon;
    public Sprite SeedIcon;
    [Header("values")]
    public float halfDevelopedTimerMax;
    public float fullDevelopedTimerMax;
    public int halfYieldMax;
    public int halfYieldMin;
    public int fullYieldMax;
    public int fullYieldMin;
}
