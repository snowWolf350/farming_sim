using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryChuteUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deliveryStatusText;
    [SerializeField] Image deliveredBackground;
    [SerializeField] Color waitingColor;

    const string WAITING = "waiting";
    
    const string DELIVERED = "Delivered";
    Color deliveredColor = Color.green;
    const string MISMATCH = "Mismatch";
    Color mismatchColor = Color.red;

    float displayTime = 0.5f;

    DeliveryChute baseDeliveryChute;

    private void Start()
    {
        baseDeliveryChute = transform.parent.GetComponent<DeliveryChute>();
        deliveredBackground.color = waitingColor;
        DeliveryChute.OnDeliverySuccess += DeliveryChute_OnDeliverySuccess;
        DeliveryChute.OnDeliveryFailed += DeliveryChute_OnDeliveryFailed;
    }

    private void DeliveryChute_OnDeliveryFailed(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        if (e.deliveryChute == baseDeliveryChute)
        {
            //this chute fired the event
            StartCoroutine(ChangeStatus(MISMATCH, mismatchColor));
        }
    }

    private void DeliveryChute_OnDeliverySuccess(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        if (e.deliveryChute == baseDeliveryChute)
        {
            StartCoroutine(ChangeStatus(DELIVERED, deliveredColor));
        }
    }

    IEnumerator ChangeStatus(string message, Color color)
    {
        deliveredBackground.color = color;
        deliveryStatusText.text = message;

        yield return new WaitForSeconds(displayTime);

        deliveredBackground.color = waitingColor;
        deliveryStatusText.text = WAITING;
    }
}
