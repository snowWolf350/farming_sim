using System;
using UnityEngine;

public class DeliveryChute : MonoBehaviour,ICanInteract
{
    [SerializeField] PlantSO plantSO;

    public static event EventHandler<OnDeliveryEventArgs> OnDeliverySuccess;
    public static event EventHandler<OnDeliveryEventArgs> OnDeliveryFailed;
    public class OnDeliveryEventArgs : EventArgs
    {
        public Plant plant;
        public DeliveryChute deliveryChute;
    }

    public void Interact(Player player)
    {
        if (player.GetEquippedInteractable() is Plant)
        {
            if (player.GetEquippedPlant().GetPlantSO() == plantSO && player.GetEquippedPlant().GetCurrentGrowthLevel() == Plant.GrowthLevel.fruit)
            {
                //player is carrying the correct fruit
                Debug.Log("correct delivery");
                OnDeliverySuccess?.Invoke(this, new OnDeliveryEventArgs
                {
                    plant = player.GetEquippedPlant(),
                    deliveryChute = this,
                });
            }
            else
            {
                OnDeliveryFailed?.Invoke(this, new OnDeliveryEventArgs
                {
                    plant = player.GetEquippedPlant(),
                    deliveryChute = this,
                });
            }
            
        }
    }

}
