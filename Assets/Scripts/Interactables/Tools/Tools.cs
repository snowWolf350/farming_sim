using System;
using UnityEngine;

public class Tools : MonoBehaviour, ICanInteract, IHasProgress 
{
    [SerializeField] ToolsSO toolsSO;

    int toolDurability;

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    private void Start()
    {
        toolDurability = toolsSO.DurabilityMax;
        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
        {
            progressNormalized = toolDurability / toolsSO.DurabilityMax,
        });
    }
    public void Interact(Player player)
    {
        if (player.inventory.CheckToolInInventory(toolsSO, out int itemIndex))
        {
            //player has this tool in his inventory
                //swap position
                player.GetEquippedTool().setParent(null);
                player.GetEquippedTool().transform.position = this.transform.position;
                player.GetEquippedTool().gameObject.SetActive(true);
                //remove from inventory and player
                player.inventory.RemoveToolInList(this);
                player.SetEquippedTool(null);
                //equip new tool
                setParent(player.GetInteractSpawn());
                player.inventory.EquipNewTool(this);
        }
        else
        {
            setParent(player.GetInteractSpawn());
            player.inventory.EquipNewTool(this);
        }
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

    public void DecreaseDurability()
    {
        if (toolDurability - toolsSO.DurabilityDecayMax < 0)
        {
            //tool broken
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

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
