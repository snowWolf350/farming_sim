using UnityEngine;

public class Plant : MonoBehaviour, ICanInteract
{
    [SerializeField] PlantSO plantSO;
    

    private void Start()
    {
    }

    public void Interact(Player player)
    {
        setParent(player.GetInteractSpawn());
        player.SetEquippedPlant(this);
    }

    public void setParent(Transform transform)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
    public PlantSO GetPlantSO()
    {
        return plantSO;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
