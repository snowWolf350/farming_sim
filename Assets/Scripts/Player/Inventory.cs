using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Inventory;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Serializable]
    public class plantItem
    {
        public Plant plant;
        public int itemCount;

        public plantItem(Plant newplant, int count)
        {
            plant = newplant;
            itemCount = count;
        }
        public void IncreaseCount(int increaseCount = 1)
        {
            itemCount+= increaseCount;
        }
        public void DecreaseCount()
        {
            if(itemCount >=1)
            itemCount--;
        }
    }

    [Serializable]
    public class toolItem
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

    public event EventHandler<onPlantItemAddedEventArgs> onPlantItemListChanged;
    public event EventHandler<onToolItemAddedEventArgs> onToolItemListChanged;

    public class onPlantItemAddedEventArgs : EventArgs
    {
        public List<plantItem> passedPlantItemList;
    }
    public class onToolItemAddedEventArgs : EventArgs
    {
        public List<toolItem> passedToolItemList;
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
        GameInput.Instance.OnPlant3Equipped += Instance_OnPlant3Equipped;
        GameInput.Instance.OnPlant4Equipped += GameInput_OnPlant4Equipped;
        GameInput.Instance.OnPlant5Equipped += GameInput_OnPlant5Equipped;
        GameInput.Instance.OnTool1Equipped += GameInput_OnTool1Equipped;
    }

    #region |---Plants---|
   
    private void GameInput_OnPlant1Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 1)
        {
            Player.Instance.SetEquippedPlant(plantItemlist[0].plant);
            toggleEqupipedItem(plantItemlist[0].plant);
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
    }

    private void GameInput_OnPlant2Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 2)
        {
            Player.Instance.SetEquippedPlant(plantItemlist[1].plant);
            toggleEqupipedItem(plantItemlist[1].plant);
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
    }
    private void Instance_OnPlant3Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 3)
        {
            Player.Instance.SetEquippedPlant(plantItemlist[2].plant);
            toggleEqupipedItem(plantItemlist[2].plant);
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
    }
    private void GameInput_OnPlant4Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 4)
        {
            Player.Instance.SetEquippedPlant(plantItemlist[3].plant);
            toggleEqupipedItem(plantItemlist[3].plant);
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
    }
    private void GameInput_OnPlant5Equipped(object sender, EventArgs e)
    {
        if (plantItemlist.Count >= 5)
        {
            Player.Instance.SetEquippedPlant(plantItemlist[4].plant);
            toggleEqupipedItem(plantItemlist[4].plant);
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
    }

    public void AddPlantInList(Plant plant, int plantItemCount = 1)
    {
        if (plant.GetCurrentGrowthLevel() == Plant.GrowthLevel.seed)
        {
            if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
            {
                //player has this plant item in inventory
                plantItemlist[itemIndex].IncreaseCount();
            }
            else
            {
                //player does not have this in his inventory adding a new one
                plantItem plantitem = new(plant, 1);
                plantItemlist.Add(plantitem);
            }
        }
        else
        {
            if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex, Plant.GrowthLevel.fruit))
            {
                //player has this plant item in inventory
                plantItemlist[itemIndex].IncreaseCount(plantItemCount);
            }
            else
            {
                //player does not have this in his inventory adding a new one
                plantItem plantitem = new(plant, plantItemCount);
                plantItemlist.Add(plantitem);
            }
        }
        onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
        {
            passedPlantItemList = plantItemlist
        });
    }

    public void RemovePlantInList(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex))
        {
            //player has this plant item in inventory
            if (plantItemlist[itemIndex].itemCount > 1)
            {
                plantItemlist[itemIndex].DecreaseCount();
            }
            else
            {
                plantItemlist.RemoveAt(itemIndex);
                Player.Instance.SetEquippedPlant(null);
            }
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
        else
        {
            Debug.LogError("tool is not in list");
        }
    }

    public void clearPlantInList(Plant plant)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex,Plant.GrowthLevel.fruit))
        {
            plantItemlist.RemoveAt(itemIndex);
            Player.Instance.SetEquippedPlant(null);
            plant.DestroySelf();
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
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

    public bool CheckPlantInInventory(PlantSO plantSO, out int itemIndex, Plant.GrowthLevel growthLevel = Plant.GrowthLevel.seed)
    {
        for (int i = 0; i < plantItemlist.Count; i++)
        {
            //cycle through inventory
            if (plantItemlist[i].plant.GetPlantSO() == plantSO && plantItemlist[i].plant.GetCurrentGrowthLevel() == growthLevel)
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

    public void EquipNewPlant(Plant plant)
    {
        Player.Instance.SetEquippedPlant(plant);
        toggleEqupipedItem(plant);

        //if not present add in list
        AddPlantInList(plant);
    }

    #endregion

    #region |---Tools---|

    private void GameInput_OnTool1Equipped(object sender, EventArgs e)
    {
        if (toolItemList.Count >= 1)
        {
            Player.Instance.SetEquippedTool(toolItemList[0].tool);
            toggleEqupipedItem(toolItemList[0].tool);
            onToolItemListChanged?.Invoke(this, new onToolItemAddedEventArgs
            {
                passedToolItemList = toolItemList
            });
        }
    }

    public void AddToolInList(Tools tool)
    {
        if (CheckToolInInventory(tool.GetToolSO(), out int itemIndex))
        {
            //player has this tool item in inventory

        }
        else
        {
            toolItem toolitem = new(tool, 1);
            toolItemList.Add(toolitem);
        }
        onToolItemListChanged?.Invoke(this, new onToolItemAddedEventArgs
        {
            passedToolItemList = toolItemList
        });
    }
    public void RemoveToolInList(Tools tool)
    {
        if (CheckToolInInventory(tool.GetToolSO(), out int itemIndex))
        {
            //player has this plant item in inventory
            if (plantItemlist[itemIndex].itemCount > 1)
            {
                plantItemlist[itemIndex].DecreaseCount();
            }
            else
            {
                plantItemlist.RemoveAt(itemIndex);
            }
            onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
            {
                passedPlantItemList = plantItemlist
            });
        }
        else
        {
            Debug.LogError("tool is not in list");
        }
    }

    public bool CheckToolInInventory(ToolsSO toolSO, out int itemIndex)
    {
        for (int i = 0; i < toolItemList.Count; i++)
        {
            //cycle through inventory
            if (toolItemList[i].tool.GetToolSO() == toolSO)
            {
                itemIndex = i;
                return true;
            }
        }
        itemIndex = 0;
        return false;
    }
    public void EquipNewTool(Tools tool)
    {
        Player.Instance.SetEquippedTool(tool);
        toggleEqupipedItem(tool);

        // add in list
        AddToolInList(tool);
    }
    #endregion


    #region|---Generic---|

    public void toggleEqupipedItem(ICanInteract iCanInteract)
    {
        if (iCanInteract is Plant)
        {
            Plant plant = iCanInteract as Plant;
            //player has equipped plant
            Player.Instance.SetEquippedPlant(plant);
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
        if (iCanInteract is Tools)
        {
            Tools tool = iCanInteract as Tools;
            //player has equipped plant
            Player.Instance.SetEquippedTool(tool);

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

    public void IncreaseItemCountAt(int itemIndex)
    {
        plantItemlist[itemIndex].IncreaseCount();
        onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
        {
            passedPlantItemList = plantItemlist
        });
    }

    public void DecreaseItemCountAt(int itemIndex)
    {
        plantItemlist[itemIndex].IncreaseCount();
        onPlantItemListChanged?.Invoke(this, new onPlantItemAddedEventArgs
        {
            passedPlantItemList = plantItemlist
        });
    }


    #endregion

}
