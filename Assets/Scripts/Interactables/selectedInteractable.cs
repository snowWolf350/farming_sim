using UnityEngine;

public class selectedInteractable : MonoBehaviour
{
    ICanInteract baseInteract;
    Outline outline;

    private void Start()
    {
        baseInteract = transform.parent.GetComponent<ICanInteract>();
        Player.OnInteractableChanged += Player_OnInteractableChanged;
        outline = GetComponent<Outline>();
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
