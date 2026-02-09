using TMPro;
using UnityEngine;

public class SeedStorageUI : MonoBehaviour
{
    SeedStorage seedStorage;
    [SerializeField] TextMeshProUGUI seedCountText;

    private void Start()
    {
        seedStorage = transform.parent.GetComponent<SeedStorage>();
        seedStorage.OnSeedCountUpdated += SeedStorage_OnSeedCountUpdated;
    }

    private void SeedStorage_OnSeedCountUpdated(object sender, System.EventArgs e)
    {
        seedCountText.text = seedStorage.GetSeedCountString();
    }
}
