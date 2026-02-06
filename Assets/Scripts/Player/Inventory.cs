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

    [Serializable]
    class toolItem
    {
        public Tools tool;
        public float itemDurability;

        public toolItem(Tools newTool, float durability)
        {
            tool = newTool;
            itemDurability = durability;
        }
        
    }

    [SerializeField]
    List<plantItem> plantItemlist;
    [SerializeField]
    List<toolItem> toolItemList;

    public event EventHandler<onPlantItemAddedEventArgs> onPlantItemAdded;

    public class onPlantItemAddedEventArgs : EventArgs
    {
        public Sprite plantSprite;
        public int plantItemCount;
        public int inventoryIndex;
    }

    private void Awake()
    {
        Instance = this;
        plantItemlist = new List<plantItem>();
        toolItemList = new List<toolItem>();
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
            Player.SetEquippedPlant(plantItemlist[0].plant);
            toggleEqupipedItem(plantItemlist[0].plant);
        }
    }

    private void GameInput_OnPlant2Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 2)
        {
            Player.SetEquippedPlant(plantItemlist[1].plant);
            toggleEqupipedItem(plantItemlist[1].plant);
        }
    }
    public void AddPlantInList(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
        {
            //player has this plant item in inventory
            plantItemlist[itemIndex].IncreaseCount();
            onPlantItemAdded?.Invoke(this, new onPlantItemAddedEventArgs
            {
                plantItemCount = plantItemlist[itemIndex].itemCount,
                plantSprite = plantItemlist[itemIndex].plant.GetPlantSO().plantIcon,
                inventoryIndex = itemIndex
            });
        }
        else
        {
            //player does not have this in his inventory adding a new one
            plantItem plantitem = new(plant, 1);
            plantItemlist.Add(plantitem);
            onPlantItemAdded?.Invoke(this, new onPlantItemAddedEventArgs
            {
                plantItemCount = plantitem.itemCount,
                plantSprite = plantitem.plant.GetPlantSO().plantIcon,
                inventoryIndex = plantItemlist.Count - 1
            });
        }
    }
    public void RemovePlantInList(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
        {
            //player has this plant item in inventory
            plantItemlist.RemoveAt(itemIndex);
        }
        else
        {
            Debug.LogError("plant is not in list");
        }
    }
    public void AddToolInList(Tools tool)
    {
        if (CheckToolInInventory(tool, out int itemIndex))
        {
            //player has this plant item in inventory
            plantItemlist[itemIndex].IncreaseCount();
        }
        else
        {
            toolItem toolitem = new(tool, 1);
            toolItemList.Add(toolitem);
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
    public bool CheckToolInInventory(Tools tool, out int itemIndex)
    {
        for (int i = 0; i < toolItemList.Count; i++)
        {
            //cycle through inventory
            if (toolItemList[i].tool == tool)
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

    public void IncreaseItemCountAt(int itemIndex)
    {
        plantItemlist[itemIndex].IncreaseCount();
        onPlantItemAdded?.Invoke(this, new onPlantItemAddedEventArgs
        {
            plantItemCount = plantItemlist[itemIndex].itemCount,
            plantSprite = plantItemlist[itemIndex].plant.GetPlantSO().plantIcon,
            inventoryIndex = itemIndex
        });
    }

    public void EquipNewPlant(Plant plant)
    {
        Player.SetEquippedPlant(plant);
        toggleEqupipedItem(plant);

        //if not present add in list
        AddPlantInList(plant);
    }

    public void EquipNewTool(Tools tool)
    {
        Player.SetEquippedTool(tool);
        toggleEqupipedItem(tool);

        // add in list
        AddToolInList(tool);
    }

    public void toggleEqupipedItem(ICanInteract iCanInteract)
    {
        if (Player.GetEquippedInteractable() is Plant)
        {
            Plant plant = iCanInteract as Plant;
            //player has equipped plant

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
        if (Player.GetEquippedInteractable() is Tools)
        {
            Tools tool = iCanInteract as Tools;
            //player has equipped plant
            Player.SetEquippedTool(tool);

            foreach (Transform child in tool.transform.parent)
            {
                if (child.GetComponent<Tools>() == tool)
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
}
