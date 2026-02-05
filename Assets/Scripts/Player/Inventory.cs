using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Serializable]
    class plantItem
    {
        public Plant plant;
        public int itemCount;

        public plantItem(Plant newplant, int count)
        {
            plant = newplant;
            itemCount = count;
        }
        public void IncreaseCount()
        {
            itemCount++;
        }
    }

    [SerializeField]
    List<plantItem> plantItemlist;

    private void Awake()
    {
        plantItemlist = new List<plantItem>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlant1Equipped += GameInput_OnPlant1Equipped;
        GameInput.Instance.OnPlant2Equipped += GameInput_OnPlant2Equipped;
    }

    private void GameInput_OnPlant1Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 1)
        {
            toggleEqupipedPlant(plantItemlist[0].plant);
        }
    }

    private void GameInput_OnPlant2Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 2)
        {
            toggleEqupipedPlant(plantItemlist[1].plant);
        }
    }
    public void AddPlantInList(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
        {
            //player has this plant item in inventory
            plantItemlist[itemIndex].IncreaseCount();
        }
        else
        {
            plantItem plantitem = new(plant, 1);
            plantItemlist.Add(plantitem);
        }
    }

    public int GetPlantItemCount(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
        {
            return plantItemlist[itemIndex].itemCount;
        }
        return 0;
    }

    public bool CheckPlantInInventory(PlantSO plantSO, out int itemIndex)
    {
        for (int i = 0; i < plantItemlist.Count; i++)
        {
            //cycle through inventory
            if (plantItemlist[i].plant.GetPlantSO() == plantSO)
            {
                itemIndex = i;
                return true;
            }
        }
        itemIndex = 0;
        return false;
    }
    public bool playerHasSeeds()
    {
        if (plantItemlist.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void IncreaseItemCountAt(int count)
    {
        plantItemlist[count].IncreaseCount();
    }

    public void EquipNewPlant(Plant plant)
    {
        toggleEqupipedPlant(plant);

        //if not present add in list
        AddPlantInList(plant);
    }

    public void toggleEqupipedPlant(Plant plant)
    {
        Player.SetEquippedPlant(plant); 

        foreach (Transform child in plant.transform.parent)
        {
            if (child.GetComponent<Plant>() == plant)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
