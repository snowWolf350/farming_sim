using UnityEngine;

public class SeedStorage : MonoBehaviour,ICanInteract
{
    [SerializeField]PlantSO plantSO;

    public void Interact(Player player)
    {
        if (player.inventory.CheckPlantInInventory(plantSO, out int itemIndex))
        {
            //player has this plant in his inventory
            player.inventory.IncreaseItemCountAt(itemIndex);
        }
        else
        {
            //spawns new seed
            GameObject plantGO = Instantiate(plantSO.plantPrefab, player.GetInteractSpawn());
            //repositions seed into the interact transform
            plantGO.transform.localPosition = Vector3.zero;
            // sets and equips the current plant
            player.inventory.EquipNewPlant(plantGO.GetComponent<Plant>());
        }
    }
    

}
