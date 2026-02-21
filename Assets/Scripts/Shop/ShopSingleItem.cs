using System;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleItem : MonoBehaviour
{
    [Serializable]
    public enum itemType
    {
        seed,
        seedStorage
    }

    public itemType thisItemType;

    [SerializeField] PlantSO plantSO;

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
            }
        });
    }

    public void SetTemplate(int lifeAmount, Sprite itemIcom)
    {
        plantIcon.sprite = itemIcom;
        lifeAmountText.text = lifeAmount.ToString();
    }
}
