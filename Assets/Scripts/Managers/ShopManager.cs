using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
   
    [SerializeField] List<ShopSO> shopSOList;

    List<SeedStorage> purchasedSeedStorageList;


    [SerializeField] TextMeshProUGUI PlayerlifeAmountText;
    [SerializeField] GameObject UI;

    [Header("containers")]
    [SerializeField] Transform SeedStorageContainer;
    [SerializeField] Transform SeedContainer;
    [SerializeField] Transform ToolsContainer;

    [Header("template")]
    [SerializeField] Transform template;

    [Header("Buttons")]
    [SerializeField] Button seedStorageButton;
    [SerializeField] Button seedButton;
    [SerializeField] Button ToolsButton;

    private void Awake()
    {
        Instance = this;
        purchasedSeedStorageList = new List<SeedStorage>();

        seedStorageButton.onClick.AddListener(() =>
        {
            SeedStorageContainer.gameObject.SetActive(true);
            SeedContainer.gameObject.SetActive(false);
            ToolsContainer.gameObject.SetActive(false);
        });
        seedButton.onClick.AddListener(() => {
            SeedStorageContainer.gameObject.SetActive(false);
            SeedContainer.gameObject.SetActive(true);
            ToolsContainer.gameObject.SetActive(false);
        });
        ToolsButton.onClick.AddListener(() => {
            SeedStorageContainer.gameObject.SetActive(false);
            SeedContainer.gameObject.SetActive(false);
            ToolsContainer.gameObject.SetActive(true);
        });
    }

    private void Start()
    {
        seedStorageButton.Select();
        Hide();
        foreach(ShopSO shopSO in shopSOList)
        {
            spawnTemplate(shopSO, shopSO.itemType);
        }
    }

    void spawnTemplate(ShopSO shopSO,ShopSingleItem.itemType itemType)
    {
        Transform itemTransform;
        switch (itemType)
        {
            case ShopSingleItem.itemType.seed:
                itemTransform = Instantiate(template, SeedContainer);
                itemTransform.GetComponent<ShopSingleItem>().SetPlantSO(shopSO.plantSO);
                break;
            case ShopSingleItem.itemType.seedStorage:
                itemTransform = Instantiate(template, SeedStorageContainer);
                itemTransform.GetComponent<ShopSingleItem>().SetPlantSO(shopSO.plantSO);
                break;
            case ShopSingleItem.itemType.tool:
                itemTransform = Instantiate(template, ToolsContainer);
                itemTransform.GetComponent<ShopSingleItem>().SetToolSO(shopSO.toolsSO);
                break;
            default: itemTransform = null; break;
        }

        itemTransform.GetComponent<ShopSingleItem>().SetItemType(itemType);
        itemTransform.GetComponent<ShopSingleItem>().SetTemplate(shopSO.itemPrice, shopSO.itemSprite);
        itemTransform.gameObject.SetActive(true);
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
            SpawnSeedStorageFromShopSOList(plantSO);
   
        }
        foreach (SeedStorage seedStorage in purchasedSeedStorageList)
        {
            if(seedStorage.GetPlantSO() == plantSO)
            {
                return;
            }
        }
        SpawnSeedStorageFromShopSOList(plantSO);
    }

    public void purchaseTools(ToolsSO toolsSO)
    {

    }

    void SpawnSeedStorageFromShopSOList(PlantSO plantSO)
    {
        foreach (ShopSO shopSO in shopSOList)
        {
            if (shopSO.itemGameobject == null) continue;
            if (shopSO.plantSO == plantSO)
            {
                //this is the storage we are trying to buy
                GameObject newSeedStorage = Instantiate(shopSO.itemGameobject, shopSO.spawnPos, Quaternion.identity);
                purchasedSeedStorageList.Add(newSeedStorage.GetComponent<SeedStorage>()); 
                return;
            }
        }
    }

   

    public void updateLifeAmountUI(int lifeamount)
    {
        PlayerlifeAmountText.text = lifeamount.ToString();
    }

    public void Hide()
    {
        UI.SetActive(false);
    }
    public void Show()
    {
        UI.SetActive(true);
    }
}
