using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownText;

    private void Start()
    {
        GameManager.Instance.OnGameStaateChanged += GameManager_OnGameStaateChanged;
    }

    private void GameManager_OnGameStaateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.CountDownIsActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Update()
    {
            countDownText.text = GameManager.Instance.GetCountDownTimer().ToString("F0");
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
