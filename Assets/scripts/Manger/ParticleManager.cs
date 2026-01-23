using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // 싱글톤.
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
    /// <summary>
    /// 파티클 재생 함수.
    /// </summary>
    /// <param name="index">재생할 파티클의 배열 인덱스</param>
    /// <param name="pos">재생할 파티클의 위치</param>
    public void PlayFX(int index, Vector3 pos)
    {
        if (index < 0 || index >= particles.Length)
        {
            return;
        }

        GameObject go = Instantiate(particles[index].gameObject, pos, Quaternion.identity);
        if (go != null)
        {
            // 파티클의 자식 오브젝트로 붙어있는 파티클 시스템들을 모두 찾아서 재생.
            ParticleSystem[] particle = go.GetComponentsInChildren<ParticleSystem>();
            if (particle != null)
            {
                for (int i = 0; i < particle.Length; i++)
                {
                    particle[i].Play();
                }
            }
            // 2초 후에 파티클 오브젝트 삭제.
            Destroy(go, 2.0f);

        }

    }
}
