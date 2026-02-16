using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] Image BarImage;
    [SerializeField] GameObject HasProgressGameObject;
    [SerializeField] Color redColor;
    [SerializeField] Color lightRedColor;

    IHasProgress HasProgress;

    Coroutine warningCoroutine;

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
        if (e.growthLevel == Plant.GrowthLevel.decaying)
        {
            if (warningCoroutine == null)
                warningCoroutine = StartCoroutine(flashWarning());
        }
        else
        {
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
                BarImage.color = Color.white; 
            }
        }
    }

    IEnumerator flashWarning()
    {
        BarImage.color = redColor;
        float t = 0;
        while (true)
        {
            t = 0;
            while (t < 1)
            {
                BarImage.color = Color.Lerp(redColor, lightRedColor, t);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t < 1)
            {
                BarImage.color = Color.Lerp(lightRedColor, redColor, t);
                t += Time.deltaTime;
                yield return null;
            }
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
