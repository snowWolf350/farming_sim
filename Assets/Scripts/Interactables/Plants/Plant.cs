using System;
using UnityEngine;

public class Plant : MonoBehaviour, ICanInteract
{
    [SerializeField] PlantSO plantSO;
    BoxCollider boxCollider;

    Vector3 seedColliderSize = new Vector3(0.3f,0.3f,0.45f);
    Vector3 seedColliderCenter = Vector3.zero;
    public Vector3 halfDevelopedColliderSize;
    public Vector3 halfDevelopedColliderCenter;
    public Vector3 fullyDevelopedColliderSize;
    public Vector3 fullyDevelopedColliderCenter;

    public static event EventHandler<OnPlantHarvestedEventArgs> OnPlantHarvested;

    public class OnPlantHarvestedEventArgs : EventArgs
    {
        public Plant plant;
    }

    public enum GrowthLevel
    {
        seed,
        halfDeveloped,
        fullDeveloped,
        fruit
    }

    GrowthLevel currentGrowthLevel;

    GameObject activeVisual;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        activeVisual = transform.GetChild(0).gameObject;
        boxCollider.size = seedColliderSize;
        boxCollider.center = seedColliderCenter;
    }

    public void Interact(Player player)
    {

        switch (currentGrowthLevel)
        {
            case GrowthLevel.seed:
                if (player.inventory.CheckPlantInInventory(plantSO, out int itemIndex))
                {
                    //player has this plant in his inventory
                    player.inventory.IncreaseItemCountAt(itemIndex);
                    DestroySelf();
                }
                else
                {
                    setParent(player.GetInteractSpawn());
                    player.inventory.EquipNewPlant(this);
                }
                break;
            case GrowthLevel.halfDeveloped:
                SetPlantGrowthLevel(GrowthLevel.fruit);
                setParent(player.GetInteractSpawn());
                Debug.Log("yield is " + UnityEngine.Random.Range(plantSO.halfYieldMin, plantSO.halfYieldMax));
                OnPlantHarvested?.Invoke(this,new OnPlantHarvestedEventArgs
                {
                    plant = this,
                });
                break;
            case GrowthLevel.fullDeveloped:
                SetPlantGrowthLevel(GrowthLevel.fruit);
                setParent(player.GetInteractSpawn());
                Debug.Log("yield is " + UnityEngine.Random.Range(plantSO.fullYieldMin, plantSO.fullYieldMax));
                OnPlantHarvested?.Invoke(this, new OnPlantHarvestedEventArgs
                {
                    plant = this,
                });
                break;
        }
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }
    public PlantSO GetPlantSO()
    {
        return plantSO;
    }

    public GrowthLevel GetCurrentGrowthLevel()
    {
        return currentGrowthLevel;
    }

    public void SetPlantGrowthLevel(GrowthLevel growthlevel)
    {
        currentGrowthLevel = growthlevel;
        switch (growthlevel)
        {
            case GrowthLevel.halfDeveloped:
                activeVisual = Instantiate(plantSO.halfDevelopedVisual, gameObject.transform);
                boxCollider.center = halfDevelopedColliderCenter;
                boxCollider.size = halfDevelopedColliderSize;
                break;
            case GrowthLevel.fullDeveloped:
                Destroy(transform.GetChild(0).gameObject);
                activeVisual = Instantiate(plantSO.fullyDevelopedVisual, gameObject.transform);
                boxCollider.center = fullyDevelopedColliderCenter;
                boxCollider.size = fullyDevelopedColliderSize;
                break;
            case GrowthLevel.fruit:
                Destroy(transform.GetChild(0).gameObject);
                activeVisual = Instantiate(plantSO.fruitVisual, gameObject.transform);
                boxCollider.enabled = false;
                break;
        }
        activeVisual.transform.localPosition = Vector3.zero;
        activeVisual.transform.localRotation = Quaternion.identity;
    }

    public void ClearVisual()
    {
        Destroy(activeVisual);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
