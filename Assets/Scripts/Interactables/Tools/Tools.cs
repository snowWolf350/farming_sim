using System;
using UnityEngine;

public class Tools : MonoBehaviour, ICanInteract
{
    public enum toolType
    {
        wateringCan
    }

    [SerializeField] toolType thistoolType;
    public void Interact(Player player)
    {
        if (player.inventory.CheckToolInInventory(this, out int itemIndex))
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

    public bool ToolIsWaterCan()
    {
        if (thistoolType == toolType.wateringCan)
        {
            return true;
        }
        return false;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
