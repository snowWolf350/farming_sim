using UnityEngine;

public class selectedInteractable : MonoBehaviour
{
    ICanInteract baseInteract;
    Outline outline;

    private void Awake()
    {
        baseInteract = GetComponentInParent<ICanInteract>();
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        Player.OnInteractableChanged += Player_OnInteractableChanged;
    }

    private void OnDisable()
    {
        Player.OnInteractableChanged -= Player_OnInteractableChanged;
    }
    private void Player_OnInteractableChanged(object sender, Player.OnInteractableChangedEventArgs e)
    {
        if (outline!= null)
        {

            if (e.Interactable == baseInteract)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            } 
        }
    }
}
