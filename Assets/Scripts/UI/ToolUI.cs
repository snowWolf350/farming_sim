using UnityEngine;
using UnityEngine.UI;
public class ToolUI : MonoBehaviour
{
    [SerializeField] Image BarImage;
    [SerializeField] GameObject HasProgressGameObject;
    [SerializeField] Color fullDurabilityColor;
    [SerializeField] Color halfDurabilityColor;
    [SerializeField] Color breakDurabilityColor;

    IHasProgress HasProgress;

    private void Start()
    {
        HasProgress = HasProgressGameObject.GetComponent<IHasProgress>();
        HasProgress.onProgressChanged += HasProgress_onProgressChanged;
        BarImage.fillAmount = 0;
        BarImage.color = fullDurabilityColor;
    }

    private void HasProgress_onProgressChanged(object sender, IHasProgress.onProgressChangedEventArgs e)
    {
        BarImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized <= .75f && e.progressNormalized > .5f)
        {
            //orange durability
            BarImage.color = halfDurabilityColor;
        }
        else if (e.progressNormalized <= .5f && e.progressNormalized >0)
        {
            BarImage.color = breakDurabilityColor;
        }
        
    }

}
