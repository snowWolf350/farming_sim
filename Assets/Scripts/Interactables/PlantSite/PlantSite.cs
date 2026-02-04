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

    float seed_fullDevelopedTimer;
    float seed_fullfDevelopedTimerMax = 5f;

    GameObject halfDevelopedGameobject;
    GameObject fullDevelopedGameobject;

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
                            activePlant.gameObject.SetActive(false);
                            //plant is watered
                            seed_halfDevelopedTimer += Time.deltaTime;
                            if (seed_halfDevelopedTimer > seed_halfDevelopedTimerMax)
                            {
                                currentGrowthLevel = GrowthLevel.halfDeveloped;
                                halfDevelopedGameobject = Instantiate(activePlant.GetPlantSO().halfDevelopedVisual, spawnTransform);
                            }
                        }
                    }
                    break;
                case GrowthLevel.halfDeveloped:
                    {
                            seed_fullDevelopedTimer += Time.deltaTime;
                            if (seed_fullDevelopedTimer > seed_fullfDevelopedTimerMax)
                            {
                                currentGrowthLevel = GrowthLevel.fullDeveloped;
                                Destroy(halfDevelopedGameobject);
                                fullDevelopedGameobject = Instantiate(activePlant.GetPlantSO().fullyDevelopedVisual, spawnTransform);
                            }
                    }
                    break;
                case GrowthLevel.fullDeveloped:
                    break;
            }
        }
        
    }

    public void Interact(Player player)
    {
        if (player.HasEquippedPlant() && activePlant == null)
        {
            //player is carrying a plant and this site is empty
            SetPlant(player.GetEquippedPlant());
        }
        if (player.HasEquippedTool() && activePlant != null)
        {
            //water plant and make it start growing
            plantIsGrowing = true;
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
