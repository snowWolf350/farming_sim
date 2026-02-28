using UnityEngine;
using UnityEngine.UI;

public class ModularProgressBarUI : MonoBehaviour
{
    [SerializeField] Image BarImage;
    [SerializeField] GameObject HasProgressGameObject;
    [SerializeField] GameObject Visuals;

    IHasProgress HasProgress;

    private void Start()
    {
        HasProgress = HasProgressGameObject.GetComponent<IHasProgress>();
        HasProgress.onProgressChanged += HasProgress_onProgressChanged;
        BarImage.fillAmount = 1;
        Show();
    }

    private void HasProgress_onProgressChanged(object sender, IHasProgress.onProgressChangedEventArgs e)
    {
        if (e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }

        BarImage.fillAmount = e.progressNormalized;
    }
    void Hide()
    {
        Visuals.SetActive(false);
    }
    void Show()
    {
        Visuals.SetActive(true);
    }
}
