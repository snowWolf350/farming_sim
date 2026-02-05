using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image BarImage;
    [SerializeField] GameObject HasProgressGameObject;

    IHasProgress HasProgress;

    private void Start()
    {
        HasProgress = HasProgressGameObject.GetComponent<IHasProgress>();
        HasProgress.onProgressChanged += HasProgress_onProgressChanged;
        BarImage.fillAmount = 0;
        Hide();
    }

    private void HasProgress_onProgressChanged(object sender, IHasProgress.onProgressChangedEventArgs e)
    {
        BarImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
