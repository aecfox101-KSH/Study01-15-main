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

        GameObject go = Instantiate(particles[index].gameObject, pos, Quaternion.identity);
        if (go != null)
        {
            ParticleSystem[] particle = go.GetComponentsInChildren<ParticleSystem>();
            if (particle != null)
            {
                for (int i = 0; i < particle.Length; i++)
                {
                    particle[i].Play();
                }
            }

            Destroy(go, 2.0f);

        }

    }
}
