using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lifeAmountText;

    public void updateLifeAmountUI(int lifeamount)
    {
        lifeAmountText.text = lifeamount.ToString();
    }
}
