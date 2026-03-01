using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource playerAudioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip successFulDeliveredAudio;
    [SerializeField] AudioClip FailedDeliveredAudio;
    [SerializeField] AudioClip OnPickUpAudio;
    [SerializeField] AudioClip OnPlantWateredAudio;
    [SerializeField] AudioClip OnPlantDisinfectedAudio;
    [SerializeField] AudioClip OnItemBroughtAudio;
    [SerializeField] AudioClip OnToolBroughtAudio;

    private void Start()
    {
        DeliveryChute.OnDeliverySuccess += DeliveryChute_OnDeliverySuccess;
        DeliveryChute.OnDeliveryFailed += DeliveryChute_OnDeliveryFailed;
        PlantSite.onPlantDisinfected += PlantSite_onPlantDisinfected;
        PlantSite.onPlantWatered += PlantSite_onPlantWatered;
        Inventory.onItemPickedUp += Inventory_onItemPickedUp;
        ShopManager.OnItemBrought += ShopManager_OnItemBrought;
        ShopManager.OnToolBrought += ShopManager_OnToolBrought;
    }

    private void ShopManager_OnToolBrought(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(OnToolBroughtAudio, new Vector3(6.9f, 0, 0),.9f);
    }

    private void ShopManager_OnItemBrought(object sender, System.EventArgs e)
    {
        playClipOneShot(OnItemBroughtAudio);
    }

    private void Inventory_onItemPickedUp(object sender, System.EventArgs e)
    {
        playClipOneShot(OnPickUpAudio);
    }

    private void PlantSite_onPlantWatered(object sender, System.EventArgs e)
    {
        playClipOneShot(OnPlantWateredAudio);
    }

    private void PlantSite_onPlantDisinfected(object sender, System.EventArgs e)
    {
        playClipOneShot(OnPlantDisinfectedAudio);
    }

    private void DeliveryChute_OnDeliveryFailed(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        playClipOneShot(FailedDeliveredAudio);
    }

    private void DeliveryChute_OnDeliverySuccess(object sender, DeliveryChute.OnDeliveryEventArgs e)
    {
        playClipOneShot(successFulDeliveredAudio);
    }

    void playClipOneShot(AudioClip clip)
    {
        playerAudioSource.pitch = Random.Range(1, 1.3f);
        playerAudioSource.PlayOneShot(clip);
    }
}
