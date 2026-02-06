using System;
using UnityEngine;

public class PlantSite : MonoBehaviour, ICanInteract, IHasProgress
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

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    private void Update()
    {
        if (activePlant != null && plantIsGrowing)
        {
            switch (currentGrowthLevel)
            {
                case GrowthLevel.seed:
                    {
                        //plant is a seed
                            activePlant.gameObject.SetActive(false);
                            //plant is watered
                            seed_halfDevelopedTimer += Time.deltaTime;

                        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                        {
                            progressNormalized = seed_halfDevelopedTimer / seed_halfDevelopedTimerMax
                        });
                            if (seed_halfDevelopedTimer > seed_halfDevelopedTimerMax)
                            {
                                currentGrowthLevel = GrowthLevel.halfDeveloped;
                                halfDevelopedGameobject = Instantiate(activePlant.GetPlantSO().halfDevelopedVisual, spawnTransform);
                            }
                    }
                    break;
                case GrowthLevel.halfDeveloped:
                    {
                            seed_fullDevelopedTimer += Time.deltaTime;

                        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                        {
                            progressNormalized = seed_fullDevelopedTimer / seed_fullfDevelopedTimerMax
                        });
                        if (seed_fullDevelopedTimer > seed_fullfDevelopedTimerMax)
                            {
                                currentGrowthLevel = GrowthLevel.fullDeveloped;
                                Destroy(halfDevelopedGameobject);
                                fullDevelopedGameobject = Instantiate(activePlant.GetPlantSO().fullyDevelopedVisual, spawnTransform);
                            }
                    }
                    break;
                case GrowthLevel.fullDeveloped:
                    onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                    break;
            }
        }
        
    }

    public void Interact(Player player)
    {
        if (Player.GetEquippedInteractable() is Plant && activePlant == null)
        {
            //player is carrying a plant and this site is empty
            player.inventory.RemovePlantInList(player.GetEquippedPlant());

            if (player.inventory.GetPlantItemCount(player.GetEquippedPlant()) > 1)
            {
                //player has multiple seeds 
                GameObject plantGO = Instantiate(player.GetEquippedPlant().GetPlantSO().plantPrefab, spawnTransform);
                SetPlant(plantGO.GetComponent<Plant>());
            }
            else
            {
                //player has one seed
                SetPlant(player.GetEquippedPlant());
                Player.SetEquippedPlant(null);
            }

        }
        if (Player.GetEquippedInteractable() is Tools && activePlant != null)
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
