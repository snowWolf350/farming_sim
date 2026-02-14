using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    List<Inventory.plantItem> deliveredPlantItemList;

    private void Awake()
    {
        deliveredPlantItemList = new List<Inventory.plantItem>();
    }

    private void Start()
    {
        DeliveryChute.OnDeliverySuccess += DeliveryChute_OnDeliverySuccess;
    }

    private void DeliveryChute_OnDeliverySuccess(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        int deliveredPlantCount = Player.Instance.inventory.GetPlantItemCount(e.plant);

        if (deliveredPlantItemList.Count >= 1)
        {
            //atleast one has been delivered
            foreach (Inventory.plantItem plantItem in deliveredPlantItemList)
            {
                if (plantItem.plant == e.plant)
                {
                    //some plant of this type is aldready delivered
                    plantItem.itemCount += deliveredPlantCount;
                }
                else
                {
                    //new plant being added to the delivery list
                    deliveredPlantItemList.Add(new Inventory.plantItem(e.plant, deliveredPlantCount));
                    break;
                }
            }
        }
        else
        {
            //delivered list is empty
            deliveredPlantItemList.Add(new Inventory.plantItem(e.plant, deliveredPlantCount));
        }

            
        Player.Instance.inventory.clearPlantInList(e.plant);
    }
}
