using UnityEngine;

public class PlantSite : MonoBehaviour, ICanInteract
{
    [SerializeField] Transform spawnTransform;

    Plant activePlant;
    bool plantIsGrowing;

    public enum GrowthLevel
    {
        seed,
        halfDeveloped,
        fullDeveloped
    }

    GrowthLevel currentGrowthLevel;

    float seed_halfDevelopedTimer;
    float seed_halfDevelopedTimerMax = 5f;

    private void Update()
    {
        if (activePlant != null)
        {
            switch (currentGrowthLevel)
            {
                case GrowthLevel.seed:
                    {
                        //plant is a seed
                        if (plantIsGrowing)
                        {
                            //plant is watered
                            seed_halfDevelopedTimer += Time.deltaTime;
                            if (seed_halfDevelopedTimer > seed_halfDevelopedTimerMax)
                            {
                                currentGrowthLevel = GrowthLevel.halfDeveloped;
                                GameObject halfDevelopedGameobject = Instantiate(activePlant.GetPlantSO().halfDevelopedVisual, spawnTransform);
                            }
                        }
                    }
                    break;
            }
        }
        
    }

    public void Interact(Player player)
    {
        if (player.HasEquippedPlant())
        {
            //player is carrying a plant
            SetPlant(player.GetEquippedPlant());    
        }
        if (player.HasEquippedTool() && activePlant != null)
        {
            //water plant and make it start growing
            plantIsGrowing = true;
            Debug.Log("Plant watered");
        }
        else
        {
            //player is not carrying a plant
        }
    }

    public void SetPlant(Plant plant)
    {
        if (currentGrowthLevel == GrowthLevel.seed)
        {
            activePlant = plant;
            activePlant.setParent(spawnTransform);
        }
    }
}
