using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance = null;

    [SerializeField]
    private ParticleSystem[] particles;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;    
    }

    public void PlayFX(int index, Vector3 pos)
    {
        if (index < 0 || index >= particles.Length)
        {
            return; 
        }
    }

}
