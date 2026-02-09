using System;
using UnityEngine;

public class Tools : MonoBehaviour, ICanInteract
{
    [SerializeField] ToolsSO toolsSO;
    public void Interact(Player player)
    {
        if (player.inventory.CheckToolInInventory(toolsSO, out int itemIndex))
        {
            //player has this tool in his inventory
            Debug.Log("player aldready has tool");
            
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

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
