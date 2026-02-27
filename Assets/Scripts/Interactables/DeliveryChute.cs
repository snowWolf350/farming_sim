using System;
using UnityEngine;

public class DeliveryChute : MonoBehaviour,ICanInteract
{
    [SerializeField] PlantSO plantSO;

    string interactText;

    public static event EventHandler<OnDeliveryEventArgs> OnDeliverySuccess;
    public static event EventHandler<OnDeliveryEventArgs> OnDeliveryFailed;
    public class OnDeliveryEventArgs : EventArgs
    {
        public Plant plant;
        public DeliveryChute deliveryChute;
    }

    private void Start()
    {
        interactText = string.Concat("Deliver " + plantSO.name);
    }

    public void Interact(Player player)
    {
        if (player.GetEquippedPlant() != null)
        {
            if (player.GetEquippedPlant().GetPlantSO() == plantSO && player.GetEquippedPlant().GetCurrentGrowthLevel() == Plant.GrowthLevel.fruit)
            {
                //player is carrying the correct fruit
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

    public string GetInteractText()
    {
        return interactText;
    }
}
