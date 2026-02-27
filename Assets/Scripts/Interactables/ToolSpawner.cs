using UnityEngine;

public class ToolSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnTransform;

    public void  spawnTool(ToolsSO toolSO)
    {
        Instantiate(toolSO.toolsPrefab, spawnTransform.position, Quaternion.identity);
    }
}
