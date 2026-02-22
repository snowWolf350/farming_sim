using System;
using TMPro;
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

    [SerializeField] Image plantIcon;

    [SerializeField] TextMeshProUGUI lifeAmountText;

    int lifeAmount;

    private void Awake()
    {
        buyButton.onClick.AddListener(()=>{
            switch (thisItemType)
            {
                case itemType.seed:
                    ShopManager.Instance.PurchaseSeeds(plantSO);
                    break;
                case itemType.seedStorage:
                    ShopManager.Instance.purchaseSeedStorage(plantSO);
                    break;
                case itemType.tool:
                    ShopManager.Instance.purchaseTools(toolSO);
                    break;
            }
        });
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
    }

    public void SetTemplate(int lifeAmount, Sprite itemIcom)
    {
        plantIcon.sprite = itemIcom;
        lifeAmountText.text = lifeAmount.ToString();
    }
}
