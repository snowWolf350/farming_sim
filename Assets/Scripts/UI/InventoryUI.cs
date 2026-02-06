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
        Inventory.Instance.onPlantItemListChanged += Inventory_onPlantItemChanged;
        ClearAllInventory();
    }


    private void Inventory_onPlantItemChanged(object sender, Inventory.onPlantItemAddedEventArgs e)
    {
        //player added item
        
        for (int i = 0; i < e.passedPlantItemList.Count; i++)
        {
            plantImageList[i].sprite = e.passedPlantItemList[i].plant.GetPlantSO().SeedIcon;
            plantCountTextList[i].text = e.passedPlantItemList[i].itemCount.ToString();
            plantImageList[i].enabled = true;
            plantCountTextList[i].enabled = true;
        }

        for (int i = e.passedPlantItemList.Count; i < plantImageList.Count; i++)
        {
            plantImageList[i].enabled = false ;
            plantCountTextList[i].enabled = false;
        }
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
