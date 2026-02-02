using UnityEngine;

public class SelectedPlantSite : MonoBehaviour
{
    [SerializeField] PlantSite basePlantSite;
    Outline outline;

    private void Start()
    {
        Player.OnInteractableChanged += Player_OnInteractableChanged;
        outline = GetComponent<Outline>();
    }

    private void Player_OnInteractableChanged(object sender, Player.OnInteractableChangedEventArgs e)
    {
        if (e.PlantSite == basePlantSite)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
