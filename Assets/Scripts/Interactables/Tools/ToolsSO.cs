using UnityEngine;
[CreateAssetMenu()]
public class ToolsSO : ScriptableObject
{
    public string toolName;
    public Sprite toolSprite;
    public int DurabilityMax;
    public int DurabilityDecayMin;
    public int DurabilityDecayMax;
}
