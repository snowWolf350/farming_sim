using System;
using UnityEngine;

public class SeedStorage : MonoBehaviour,ICanInteract
{
    [SerializeField]PlantSO plantSO;

    public int startingSeedsCount;
    public int maxSeedCapacity;
    int currentSeedsCount;

    public event EventHandler OnSeedCountUpdated;

    private void Start()
    {
        currentSeedsCount = startingSeedsCount;
        OnSeedCountUpdated?.Invoke(this, new EventArgs());
    }

    public void Interact(Player player)
    {
        if (HasSeedsLeft())
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
                plantGO.transform.localRotation = Quaternion.identity;
                // sets and equips the current plant
                player.inventory.EquipNewPlant(plantGO.GetComponent<Plant>());
            }
            DecreaseSeeds(1);
            OnSeedCountUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool HasSeedsLeft()
    {
        if (currentSeedsCount > 0)
        {
            return true;
        }
        return false;
    }

    public void IncreaseSeeds(int seedCount)
    {
        currentSeedsCount += seedCount;
        OnSeedCountUpdated?.Invoke(this, new EventArgs());
    }
    public void DecreaseSeeds(int seedCount)
    {
        currentSeedsCount -= seedCount;
        OnSeedCountUpdated?.Invoke(this, new EventArgs());
    }

    public string GetSeedCountString()
    {
        return new string(currentSeedsCount.ToString() + "\\" + maxSeedCapacity.ToString());
    }

    public float GetHoldDuration()
    {
        return 0;
    }

    public bool RequiresHold()
    {
        return false;
    }
}
