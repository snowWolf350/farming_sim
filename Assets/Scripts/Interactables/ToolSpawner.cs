using UnityEngine;

public class ToolSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnTransform;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem sparkParticleSystem;
    const string ACTIVATE = "activate";


    public void  spawnTool(ToolsSO toolSO)
    {
        Instantiate(toolSO.toolsPrefab, spawnTransform.position, Quaternion.identity);
        animator.SetTrigger(ACTIVATE);
        sparkParticleSystem.Play();
    }
}
