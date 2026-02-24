using System;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleItem : MonoBehaviour
{
    [Serializable]
    public enum itemType
    {
        seed,
        seedStorage,
        tool
    }

    public itemType thisItemType;

    [SerializeField] PlantSO plantSO;

    [SerializeField] ToolsSO toolSO;

    [SerializeField] Button buyButton;
    [SerializeField] Button IncreaseButton;
    [SerializeField] Button DecreaseButton;

    [SerializeField] Image plantIcon;

    [SerializeField] TextMeshProUGUI lifeAmountText;
    [SerializeField] TextMeshProUGUI itemAmountText;

    int lifeAmount;
    int totalLifeAmount;
    int itemAmount = 0;

    private void Awake()
    {
        buyButton.onClick.AddListener(()=>{
            switch (thisItemType)
            {
                case itemType.seed:
                    ShopManager.Instance.PurchaseSeeds(plantSO,totalLifeAmount,itemAmount);
                    itemAmount = 0;
                    itemAmountText.text = string.Concat("Buy " + itemAmount.ToString());
                    totalLifeAmount = 0;
                    break;
                case itemType.seedStorage:
                    ShopManager.Instance.purchaseSeedStorage(plantSO,lifeAmount);
                    break;
                case itemType.tool:
                    ShopManager.Instance.purchaseTools(toolSO);
                    break;
            }
        });
            IncreaseButton.onClick.AddListener(() =>
            {
                if (ItemAmountWithinBounds())
                {
                    itemAmount++;
                    totalLifeAmount = lifeAmount * itemAmount;
                    itemAmountText.text = string.Concat("Buy " + itemAmount.ToString());
                }
            });
            DecreaseButton.onClick.AddListener(() =>
            {
                if (ItemAmountWithinBounds())
                {
                    itemAmount--;
                    totalLifeAmount = lifeAmount * itemAmount;
                    itemAmountText.text = string.Concat("Buy " + itemAmount.ToString());
                }
            });
        
    }

    bool ItemAmountWithinBounds()
    {
        if (itemAmount > 1 || totalLifeAmount < DeliveryManager.Instance.GetCurrentLifeAmount())
        {
            return true;
        }
        return false;
    }

    public void SetPlantSO(PlantSO plantSO)
    {
        this.plantSO = plantSO;
    }

    public void SetToolSO(ToolsSO toolsSO)
    {
        this.toolSO = toolsSO;
    }

    public void SetItemType(itemType type)
    {
        thisItemType = type;
        if(type == itemType.seed)
        {
            IncreaseButton.gameObject.SetActive(true);
            DecreaseButton.gameObject.SetActive(true);
        }
        else
        {
            IncreaseButton.gameObject.SetActive(false);
            DecreaseButton.gameObject.SetActive(false);
        }
    }

    public void SetTemplate(int lifeAmount, Sprite itemIcom)
    {
        plantIcon.sprite = itemIcom;
        if(plantSO != null)
        {
            lifeAmountText.text = string.Concat(plantSO.plantName +":" + lifeAmount.ToString());
        }
        else if (toolSO != null)
        {
            lifeAmountText.text = string.Concat(toolSO.toolName + ":" + lifeAmount.ToString());
        }
        else
        {
            lifeAmountText.text = string.Concat(plantSO.plantName + "storage:" + lifeAmount.ToString());
        }

        this.lifeAmount = lifeAmount;

    }
}
