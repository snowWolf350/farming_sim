using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DeliveryManager;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    List<DeliverdItem> deliveredPlantItemList;

    [Serializable]
    public class DeliverdItem
    {
        public PlantSO plantSO;
        public int itemCount;

        public DeliverdItem( PlantSO plantSO, int itemCount)
        {
            this.plantSO = plantSO;
            this.itemCount = itemCount;
        }
    }

    [Header("Templaate UI")]
    [SerializeField] Transform template;
    [SerializeField] Transform container;
    List<Transform> templateList;

    //shop
    [Header("Shop UI")]
    int lifeAmount = 0;

    [SerializeField] TextMeshProUGUI PlayerlifeAmountText;
    [SerializeField] TextMeshProUGUI TotallifeAmountText;

    int lifeAmountDelivered;

    const int LifeAmountRequired = 1000;

    private void Awake()
    {
        deliveredPlantItemList = new List<DeliverdItem>();
        templateList = new List<Transform>();
        Instance = this;
        updateLifeAmountUI();
    }

    private void Start()
    {
        DeliveryChute.OnDeliverySuccess += DeliveryChute_OnDeliverySuccess;
    }

    private void DeliveryChute_OnDeliverySuccess(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        int deliveredPlantCount = Inventory.Instance.GetPlantItemCount(e.plant,Plant.GrowthLevel.fruit);
        int deliveredPlantCost = deliveredPlantCount * e.plant.GetPlantSO().lifeAmount;

        if (deliveredPlantItemList.Count >= 1)
        {
            bool plantIsThere = false;
            //atleast one has been delivered
            foreach (DeliverdItem deliverdItem in deliveredPlantItemList)
            {
                if (deliverdItem.plantSO == e.plant.GetPlantSO())
                {
                    //some plant of this type is aldready delivered
                    deliverdItem.itemCount += deliveredPlantCount;
                    UpdateTemplateCount(deliverdItem.itemCount, deliverdItem.plantSO);
                    UpdateLifeAmount(deliveredPlantCost);
                    plantIsThere = true;
                    break;
                }
            }
            if (!plantIsThere)
            {
                //new plant being added to the delivery list
                deliveredPlantItemList.Add(new DeliverdItem(e.plant.GetPlantSO(), deliveredPlantCount));
                CreateNewTemplate(deliveredPlantCount, e.plant.GetPlantSO().plantIcon);
                UpdateLifeAmount(deliveredPlantCost);
            }
        }
        else
        {
            //delivered list is empty
            deliveredPlantItemList.Add(new DeliverdItem(e.plant.GetPlantSO(), deliveredPlantCount));
            CreateNewTemplate(deliveredPlantCount, e.plant.GetPlantSO().plantIcon);
            UpdateLifeAmount(deliveredPlantCost);
        }

            
        Player.Instance.inventory.clearPlantInList(e.plant);
    }

    void UpdateLifeAmount(int itemCost)
    {
        lifeAmount += itemCost;
        lifeAmountDelivered += itemCost;
        updateLifeAmountUI();
    }

    public void DecreaseLifeAmount(int itemCost)
    {
        lifeAmount -= itemCost;
        updateLifeAmountUI();
    }

    public bool CheckRequiredLifeAmount()
    {
        if (lifeAmountDelivered > LifeAmountRequired)
        {
            return true;
        }
        return false;
    }

    void CreateNewTemplate(int deliverCount,Sprite plantSprite)
    {
        Transform templateTransform = Instantiate(template, container);
        templateTransform.gameObject.SetActive(true);
        if (templateTransform.TryGetComponent(out MonitorSingleUI monitorSingleUI))
        {
            monitorSingleUI.SetDeliveredCount(deliverCount);
            monitorSingleUI.SetPlantImage(plantSprite);
        }
        templateList.Add(templateTransform);
    }
    void UpdateTemplateCount(int deliverCount, PlantSO plantSO)
    {
        Transform templateTransform = templateList[GetDeliveredItemIndex(plantSO)];
        if (templateTransform.TryGetComponent(out MonitorSingleUI monitorSingleUI))
        {
            monitorSingleUI.SetDeliveredCount(deliverCount);
        }
    }

    public int GetCurrentLifeAmount()
    {
        return lifeAmount;
    }


    int GetDeliveredItemIndex(PlantSO plantSO)
    {
        for (int i = 0; i < deliveredPlantItemList.Count; i++)
        {
            if (plantSO == deliveredPlantItemList[i].plantSO)
            {
                return i;
            }
        }
        return 0;
    }

    public List<DeliverdItem> GetDeliveredList()
    {
        return deliveredPlantItemList;
    }

    public void updateLifeAmountUI()
    {
        PlayerlifeAmountText.text = lifeAmount.ToString();
        TotallifeAmountText.text = string.Concat("Total:" + lifeAmountDelivered.ToString() + "/" + LifeAmountRequired.ToString());
    }


    [ContextMenu("increase")]
    void setLifeAmount()
    {
        lifeAmount = 200;
    }
}
