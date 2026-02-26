using TMPro;
using UnityEngine;

public class InteractBoxUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;

    void Start()
    {
        Player.OnInteractableChanged += Player_OnInteractableChanged;
        Hide();
    }

    private void Player_OnInteractableChanged(object sender, Player.OnInteractableChangedEventArgs e)
    {
        if (e.Interactable == null)
        {
            Hide();
            return;
        }
        Show();
        interactText.text = e.Interactable.GetInteractText();
    }

    void Hide()
    {
        interactText.gameObject.SetActive(false);
    }
    void Show()
    {
        interactText.gameObject.SetActive(true);
    }
}
