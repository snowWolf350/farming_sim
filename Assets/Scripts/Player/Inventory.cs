using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Inventory;
using static UnityEngine.GridBrushBase;

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

    [SerializeField]
    List<plantItem> plantItemlist;
    [SerializeField]
    List<Tools> toolItemList;

    public event EventHandler<onPlantItemAddedEventArgs> onPlantItemListChanged;
    public event EventHandler<onToolItemAddedEventArgs> onToolItemListChanged;

    public class onPlantItemAddedEventArgs : EventArgs
    {
        public List<plantItem> passedPlantItemList;
    }
    public class onToolItemAddedEventArgs : EventArgs
    {
        public List<Tools> passedToolItemList;
    }

    private void Awake()
    {
        Instance = this;
        plantItemlist = new List<plantItem>();
        toolItemList = new List<Tools>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlant1Equipped += GameInput_OnPlant1Equipped;
        GameInput.Instance.OnPlant2Equipped += GameInput_OnPlant2Equipped;
        GameInput.Instance.OnPlant3Equipped += Instance_OnPlant3Equipped;
        GameInput.Instance.OnPlant4Equipped += GameInput_OnPlant4Equipped;
        GameInput.Instance.OnPlant5Equipped += GameInput_OnPlant5Equipped;
        GameInput.Instance.OnTool1Equipped += GameInput_OnTool1Equipped;
        GameInput.Instance.OnTool2Equipped += GameInput_OnTool2Equipped;
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
                //player has this seed item in inventory
                plantItemlist[itemIndex].IncreaseCount();
            }
            else
            {
                //player does not have this in his inventory adding a new one, seed
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
        toggleEqupipedItem(plant);
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
            Debug.LogError("plant is not in list");
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

    public int GetPlantItemCount(Plant plant,Plant.GrowthLevel growthLevel = Plant.GrowthLevel.seed)
    {
        if (CheckPlantInInventory(plant.GetPlantSO(), out int itemIndex,growthLevel))
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
            Player.Instance.SetEquippedTool(toolItemList[0]);
            toggleEqupipedItem(toolItemList[0]);
            onToolItemListChanged?.Invoke(this, new onToolItemAddedEventArgs
            {
                passedToolItemList = toolItemList
            });
        }
    }
    private void GameInput_OnTool2Equipped(object sender, EventArgs e)
    {
        if (toolItemList.Count >= 2)
        {
            Player.Instance.SetEquippedTool(toolItemList[1]);
            toggleEqupipedItem(toolItemList[1]);
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
            Tools equippedTool = toolItemList[itemIndex];

            equippedTool.setParent(null);
            equippedTool.transform.position = tool.transform.position;
            equippedTool.GetRigidbody().isKinematic = false;

            RemoveToolInList(equippedTool);

            tool.setParent(Player.Instance.GetInteractSpawn());
            tool.GetRigidbody().isKinematic = true;

            BoxCollider collider = tool.gameObject.GetComponent<BoxCollider>();
            collider.enabled = false;
            toolItemList.Add(tool);

            Player.Instance.SetEquippedTool(tool);
            toggleEqupipedItem(tool);
        }
        else
        {
            tool.setParent(Player.Instance.GetInteractSpawn());
            tool.GetRigidbody().isKinematic = true;
            BoxCollider collider = tool.gameObject.GetComponent<BoxCollider>();
            collider.enabled = false;
            toolItemList.Add(tool);

            Player.Instance.SetEquippedTool(tool);
            toggleEqupipedItem(tool);
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
            BoxCollider collider = tool.gameObject.GetComponent<BoxCollider>();
            collider.enabled = true;
            //player has this tool item in inventory
            toolItemList.RemoveAt(itemIndex);

            onToolItemListChanged?.Invoke(this, new onToolItemAddedEventArgs
            {
                passedToolItemList = toolItemList
            });
        }
    }


    public void clearToolInList(Tools tool)
    {
        if (CheckToolInInventory(tool.GetToolSO(), out int itemIndex))
        {
            toolItemList.RemoveAt(itemIndex);
            Player.Instance.SetEquippedTool(null);
            tool.DestroySelf();
            onToolItemListChanged?.Invoke(this, new onToolItemAddedEventArgs
            {
                passedToolItemList = toolItemList
            });
        }
    }


    public bool CheckToolInInventory(ToolsSO toolSO, out int itemIndex)
    {
        for (int i = 0; i < toolItemList.Count; i++)
        {
            //cycle through inventory
            if (toolItemList[i].GetToolSO() == toolSO)
            {
                itemIndex = i;
                return true;
            }
        }
        itemIndex = 0;
        return false;
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
            Player.Instance.SetEquippedTool(null);
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
            Player.Instance.SetEquippedPlant(null);

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
