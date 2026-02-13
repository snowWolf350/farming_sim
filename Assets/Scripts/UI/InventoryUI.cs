using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Plants")]
    [SerializeField] List<Image> plantImageList;
    [SerializeField] List<Image> plantImageBackgroundList;
    public Color equippedBackgroundColor;
    [SerializeField] List<TextMeshProUGUI> plantCountTextList;
    [Header("Tools")]
    [SerializeField] List<Image> toolImageList;
    [SerializeField] List<Image> ToolImageBackgroundList;

    private void Start()
    {
        Inventory.Instance.onPlantItemListChanged += Inventory_onPlantItemChanged;
        Inventory.Instance.onToolItemListChanged += Inventory_onToolItemListChanged;
        ClearPlantInventory();
        ClearToolInventory();
    }

    private void Inventory_onToolItemListChanged(object sender, Inventory.onToolItemAddedEventArgs e)
    {
        for (int i = 0; i < e.passedToolItemList.Count; i++)
        {
            toolImageList[i].sprite = e.passedToolItemList[i].tool.GetToolSO().toolSprite;
            toolImageList[i].enabled = true;
        }

        for (int i = e.passedToolItemList.Count; i < toolImageList.Count; i++)
        {
            toolImageList[i].enabled = false;
        }
        for(int i = 0; i < e.passedToolItemList.Count; i++)
        {
            if (e.passedToolItemList[i].tool == Player.Instance.GetEquippedInteractable() as Tools)
            {
                //player added tool
                ToolImageBackgroundList[i].color = equippedBackgroundColor;
                ClearPlantInventoryBackground();
            }
            else
            {
                ToolImageBackgroundList[i].color = Color.black;
            }
        }
        for (int i = e.passedToolItemList.Count; i < ToolImageBackgroundList.Count; i++)
        {
            ToolImageBackgroundList[i].color = Color.black;
        }
    }

    private void Inventory_onPlantItemChanged(object sender, Inventory.onPlantItemAddedEventArgs e)
    {
        //player added item
        
        for (int i = 0; i < e.passedPlantItemList.Count; i++)
        {
            if (e.passedPlantItemList[i].plant.GetCurrentGrowthLevel() == Plant.GrowthLevel.seed)
            {
                plantImageList[i].sprite = e.passedPlantItemList[i].plant.GetPlantSO().SeedIcon;
            }
            else
            {
                plantImageList[i].sprite = e.passedPlantItemList[i].plant.GetPlantSO().plantIcon;
            }
                plantCountTextList[i].text = e.passedPlantItemList[i].itemCount.ToString();
            plantImageList[i].enabled = true;
            plantCountTextList[i].enabled = true;
        }

        for (int i = e.passedPlantItemList.Count; i < plantImageList.Count; i++)
        {
            plantImageList[i].enabled = false ;
            plantCountTextList[i].enabled = false;
        }

        for (int i = 0; i < e.passedPlantItemList.Count; i++)
        {
            if (e.passedPlantItemList[i].plant == Player.Instance.GetEquippedInteractable() as Plant)
            {
                //player has a equipped a plant
                plantImageBackgroundList[i].color = equippedBackgroundColor;
                ClearToolInventoryBackground();
            }
            else
            {
                plantImageBackgroundList[i].color = Color.black;
            }
        }
        for (int i = e.passedPlantItemList.Count; i < plantImageBackgroundList.Count; i++)
        {
            plantImageBackgroundList[i].color = Color.black;
        }

    }

    void ClearPlantInventoryBackground()
    {
        foreach (Image image in plantImageBackgroundList)
        {
            image.color = Color.black;
        }
    }
    void ClearToolInventoryBackground()
    {
        foreach (Image image in ToolImageBackgroundList)
        {
            image.color = Color.black;
        }
    }

    void ClearPlantInventory()
    {
        foreach (Image image in plantImageList)
        {
            image.enabled = false;
        }
        foreach (TextMeshProUGUI text in plantCountTextList)
        {
            text.enabled = false;
        }
        
    }
    void ClearToolInventory()
    {
        foreach (Image image in toolImageList)
        {
            image.enabled = false;
        }
    }
}
