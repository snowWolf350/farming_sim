using System;
using UnityEngine;

public class DeliveryChute : MonoBehaviour,ICanInteract
{
    [SerializeField] PlantSO plantSO;

    public static event EventHandler<OnDeliverySuccessEventArgs> OnDeliverySuccess;
    public class OnDeliverySuccessEventArgs : EventArgs
    {
        public Plant plant;
    }

    public void Interact(Player player)
    {
        if (player.GetEquippedInteractable() is Plant)
        {
            if (player.GetEquippedPlant().GetPlantSO() == plantSO && player.GetEquippedPlant().GetCurrentGrowthLevel() == Plant.GrowthLevel.fruit)
            {
                //player is carrying the correct fruit
                Debug.Log("correct delivery");
                OnDeliverySuccess?.Invoke(this, new OnDeliverySuccessEventArgs
                {
                    plant = player.GetEquippedPlant()
                });
            }
            else
            {
                Debug.Log("wrong delivery");
            }
            
        }
    }
}
