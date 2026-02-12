using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlantSite : MonoBehaviour, ICanInteract, IHasProgress
{
    [SerializeField] Transform spawnTransform;

    Plant activePlant;
    bool plantIsGrowing;

    float seed_halfDevelopedTimer;
    float seed_fullDevelopedTimer;

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    private void Start()
    {
        Plant.OnPlantHarvested += Plant_OnPlantHarvested;
    }

    private void Plant_OnPlantHarvested(object sender, Plant.OnPlantHarvestedEventArgs e)
    {
        if (activePlant == e.plant)
        {
            //plant removed from this site 
            plantIsGrowing = false;
            activePlant = null;
            onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
            {
                progressNormalized = 0
            });
            seed_fullDevelopedTimer = 0;
            seed_halfDevelopedTimer = 0;
        }
    }

    private void Update()
    {
        if (activePlant != null && plantIsGrowing)
        {
            switch (activePlant.GetCurrentGrowthLevel())
            {
                case Plant.GrowthLevel.seed:
                    {
                        //plant is a seed
                        if (seed_halfDevelopedTimer == 0)
                        {
                            activePlant.ClearVisual();
                        }
                         //plant is watered
                        seed_halfDevelopedTimer += Time.deltaTime;

                        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                        {
                            progressNormalized = seed_halfDevelopedTimer / activePlant.GetPlantSO().halfDevelopedTimerMax
                        });
                        if (seed_halfDevelopedTimer > activePlant.GetPlantSO().halfDevelopedTimerMax)
                        {
                           activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.halfDeveloped);
                           seed_halfDevelopedTimer = 0;
                        }
                    }
                    break;
                case Plant.GrowthLevel.halfDeveloped:
                    {
                            seed_fullDevelopedTimer += Time.deltaTime;

                            onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                            {
                            progressNormalized = seed_fullDevelopedTimer / activePlant.GetPlantSO().fullDevelopedTimerMax
                            });
                            if (seed_fullDevelopedTimer > activePlant.GetPlantSO().fullDevelopedTimerMax)
                            {
                              activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.fullDeveloped);
                              seed_fullDevelopedTimer = 0;
                            }
                    }
                    break;
                case Plant.GrowthLevel.fullDeveloped:
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
        if (Player.Instance.GetEquippedInteractable() is Plant && activePlant == null)
        {
            //player is carrying a plant and this site is empty
            if (player.inventory.GetPlantItemCount(player.GetEquippedPlant()) > 1)
            {
                //player has multiple seeds 
                GameObject plantGO = Instantiate(player.GetEquippedPlant().GetPlantSO().plantPrefab, spawnTransform);
                SetPlant(plantGO.GetComponent<Plant>());
                player.inventory.RemovePlantInList(player.GetEquippedPlant());
            }
            else
            {
                //player has one seed

                SetPlant(player.GetEquippedPlant());
                player.inventory.RemovePlantInList(player.GetEquippedPlant());
            }

        }
        if (Player.Instance.GetEquippedInteractable() is Tools && activePlant != null)
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
        if (plant.GetCurrentGrowthLevel() == Plant.GrowthLevel.seed)
        {
            activePlant = plant;
            activePlant.setParent(spawnTransform);
        }
    }
}
