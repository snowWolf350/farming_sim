using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonitorSingleUI : MonoBehaviour
{
    [SerializeField] Image plantImage;
    [SerializeField] TextMeshProUGUI DeliveredText;

    public void SetPlantImage(Sprite sprite)
    {
        plantImage.sprite = sprite;
    }

    public void SetDeliveredCount(int count)
    {
        DeliveredText.text = count.ToString();
    }
}
