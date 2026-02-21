using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlantSite : MonoBehaviour, ICanInteract, IHasProgress
{
    [SerializeField] Transform spawnTransform;
    [SerializeField] ParticleSystem decayingParticleSystem;
    [SerializeField] ToolsSO wateringCan;
    [SerializeField] ToolsSO sprinkler;

    Plant activePlant;
    bool plantIsGrowing;

    float globalTimer;

    public event EventHandler<IHasProgress.onProgressChangedEventArgs> onProgressChanged;

    private void Start()
    {
        Plant.OnPlantHarvested += Plant_OnPlantHarvested;
        decayingParticleSystem.Stop();
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
            globalTimer = 0;
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
                        if (globalTimer == 0)
                        {
                            activePlant.ClearVisual();
                        }
                         //plant is watered
                        globalTimer += Time.deltaTime;

                        onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                        {
                            progressNormalized = globalTimer / activePlant.GetPlantSO().halfDevelopedTimerMax
                        });
                        if (globalTimer > activePlant.GetPlantSO().halfDevelopedTimerMax)
                        {
                           activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.halfDeveloped);
                           globalTimer = 0;
                        }
                    }
                    break;
                case Plant.GrowthLevel.halfDeveloped:
                    {
                            globalTimer += Time.deltaTime;

                            onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                            {
                                progressNormalized = globalTimer / activePlant.GetPlantSO().fullDevelopedTimerMax
                            });
                            if (globalTimer > activePlant.GetPlantSO().fullDevelopedTimerMax)
                            {
                               if (UnityEngine.Random.Range(activePlant.GetPlantSO().DecayChanceMin, activePlant.GetPlantSO().DecayChanceMax) <= activePlant.GetPlantSO().DecayChance)
                               {
                                //plant is decayed
                                  activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.decaying);
                                  globalTimer = 0;
                               }
                               else
                               {
                                //plant is healthy
                                  activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.fullDeveloped);
                                  globalTimer = 0;
                               }
                               
                            }
                    }
                    break;
                case Plant.GrowthLevel.decaying:
                    decayingParticleSystem.Play();
                    globalTimer += Time.deltaTime;
                    onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                    {
                        progressNormalized = globalTimer / activePlant.GetPlantSO().decayingTimerMax,
                        growthLevel = activePlant.GetCurrentGrowthLevel()
                    });

                    if (globalTimer > activePlant.GetPlantSO().decayingTimerMax)
                    {
                        globalTimer = 0;
                        activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.destroyed);
                    }
                    break;
                case Plant.GrowthLevel.fullDeveloped:
                    decayingParticleSystem.Stop();
                    onProgressChanged?.Invoke(this, new IHasProgress.onProgressChangedEventArgs
                    {
                        progressNormalized = 0
                    });
                    break;
                case Plant.GrowthLevel.destroyed:
                    decayingParticleSystem.Stop();
                    clearActivePlant();
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
        if (Player.Instance.GetEquippedInteractable() is Tools && activePlant != null )
        {
            //water plant and make it start growing
            if (player.GetEquippedTool().GetToolSO() == wateringCan && !plantIsGrowing)
            {
                //player has a watering can 
                player.GetEquippedTool().DecreaseDurability();
                plantIsGrowing = true;
            }
            else
            {
                //player has sprinkler
                if (activePlant.GetCurrentGrowthLevel() == Plant.GrowthLevel.decaying && player.GetEquippedTool().GetToolSO() == sprinkler)
                {
                    //plant is decaying and player has sprinkler
                    activePlant.SetPlantGrowthLevel(Plant.GrowthLevel.fullDeveloped);
                }
            }
        }
        else
        {
            //player is not carrying a plant
            return;
        }
    }

    void clearActivePlant()
    {
        activePlant.DestroySelf();
        activePlant = null;
        plantIsGrowing = false;
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
