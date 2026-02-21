using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
   
    [SerializeField] List<ShopSO> shopSOList;

    List<SeedStorage> purchasedSeedStorageList;


    [SerializeField] TextMeshProUGUI lifeAmountText;

    private void Awake()
    {
        Instance = this;
        purchasedSeedStorageList = new List<SeedStorage>();
    }

    public void PurchaseSeeds(PlantSO plantSO)
    {
        if (purchasedSeedStorageList.Count == 0) return;

        foreach(SeedStorage seedStorage in purchasedSeedStorageList)
        {
            if(seedStorage.GetPlantSO() == plantSO)
            {
                //there is a storage of this seed
                seedStorage.IncreaseSeeds(1);
            }
        }
    }

    public void purchaseSeedStorage(PlantSO plantSO)
    {
        //if seed storage is there, else spawn a new seed storage store it in purchasedstorage list
        if(purchasedSeedStorageList.Count == 0)
        {
            //not purchased anything yet
            SpawnFromShopSOList(plantSO);
   
        }
        foreach (SeedStorage seedStorage in purchasedSeedStorageList)
        {
            if(seedStorage.GetPlantSO() == plantSO)
            {
                return;
            }
        }
        SpawnFromShopSOList(plantSO);
    }

    void SpawnFromShopSOList(PlantSO plantSO)
    {
        foreach (ShopSO shopSO in shopSOList)
        {
            if (shopSO.itemGameobject == null) continue;
            if (shopSO.plantSO == plantSO)
            {
                //this is the storage we are trying to buy
                GameObject newSeedStorage = Instantiate(shopSO.itemGameobject, shopSO.spawnPos, Quaternion.identity);
                purchasedSeedStorageList.Add(newSeedStorage.GetComponent<SeedStorage>());
                Debug.Log("spawned box");   
                return;
            }
        }
    }

    public void updateLifeAmountUI(int lifeamount)
    {
        lifeAmountText.text = lifeamount.ToString();
    }
}
