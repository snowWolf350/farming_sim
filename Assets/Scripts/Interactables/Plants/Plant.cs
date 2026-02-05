using UnityEngine;

public class Plant : MonoBehaviour, ICanInteract
{
    [SerializeField] PlantSO plantSO;
    

    private void Start()
    {
    }

    public void Interact(Player player)
    {
        if (player.inventory.CheckPlantInInventory(plantSO, out int itemIndex))
        {
            //player has this plant in his inventory
            player.inventory.IncreaseItemCountAt(itemIndex);
            DestroySelf();
        }
        else
        {
            setParent(player.GetInteractSpawn());
            player.inventory.EquipNewPlant(this);
        }
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
    public PlantSO GetPlantSO()
    {
        return plantSO;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
