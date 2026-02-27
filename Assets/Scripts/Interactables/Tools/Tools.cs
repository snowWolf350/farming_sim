using System;
using UnityEngine;

public class Tools : MonoBehaviour, ICanInteract, IHasProgress 
{
    [SerializeField] ToolsSO toolsSO;

    Rigidbody toolRigidbody;

    int toolDurability;

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    string interactText;

    private void Start()
    {
        toolDurability = toolsSO.DurabilityMax;
        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
        {
            progressNormalized = toolDurability / toolsSO.DurabilityMax,
        });
        interactText = string.Concat( "Pick up" + toolsSO.toolName);
        this.toolRigidbody = GetComponent<Rigidbody>();
    }
    public void Interact(Player player)
    {
        player.inventory.AddToolInList(this);
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }

    public ToolsSO GetToolSO()
    {
        return toolsSO;
    }

    public int GetToolDurability()
    {
        return toolDurability;
    }
    public Rigidbody GetRigidbody()
    {
        return toolRigidbody;
    }

    [ContextMenu("decrease durability")]
    public void DecreaseDurability()
    {
        if (toolDurability - toolsSO.DurabilityDecayMax < 0)
        {
            //tool broken
            Player.Instance.inventory.clearToolInList(this);
        }
        else
        {
            //decrease durability
            toolDurability -= (int)UnityEngine.Random.Range(toolsSO.DurabilityDecayMin, toolsSO.DurabilityDecayMax);
            onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
            {
                progressNormalized = (float)toolDurability / toolsSO.DurabilityMax,
            });
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
