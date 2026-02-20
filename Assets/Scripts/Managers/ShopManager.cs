using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public class ShopItem
    {
        public ShopSO shopSO;
        public bool isPurchased;
    }

    [SerializeField] List<ShopItem> shopItemList;
}
