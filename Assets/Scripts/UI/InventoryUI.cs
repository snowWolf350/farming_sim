using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] List<Image> plantImageList;
    [SerializeField] List<TextMeshProUGUI> plantCountTextList;

    private void Start()
    {
        Inventory.Instance.onPlantItemAdded += Inventory_onPlantItemAdded;
        ClearAllInventory();
    }

    private void Inventory_onPlantItemAdded(object sender, Inventory.onPlantItemAddedEventArgs e)
    {
        //player added item
        plantImageList[e.inventoryIndex].sprite = e.plantSprite;
        plantImageList[e.inventoryIndex].enabled = true;
        plantCountTextList[e.inventoryIndex].text = e.plantItemCount.ToString();
        plantCountTextList[e.inventoryIndex].enabled = true;
    }

    void ClearAllInventory()
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
}
