using UnityEngine;
public enum AudioType
{
    Jump = 0,
    GainCoin = 1,
    BGM_Main = 2
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSFX;

    [SerializeField]
    private AudioSource audioBGM;

    [SerializeField]
    private AudioClip[] clips;

    public static AudioManager instance = null;

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void  PlaySFX(AudioType index)
    {
        if ((int)index <  0 || (int)index >= clips.Length)
        {
            return;
        }
        audioSFX.clip = clips[(int)index];
        audioSFX.Play();
       
    }
    public void PlayBGM(AudioType index)
    {
        if ((int)index < 0 || (int)index >= clips.Length)
        { 
            return;
        }

        // 현재 나오는 음악과 같으면 다시 틀지 않음 (중복 방지)
        if (audioBGM.clip == clips[(int)index] && audioBGM.isPlaying) 
        {
            return;
        }

        audioBGM.clip = clips[(int)index];
        audioBGM.loop = true; // BGM은 무한 반복
        audioBGM.Play();
    }

    // BGM 정지 기능 (필요할 때 호출)
    public void StopBGM()
    {
        audioBGM.Stop();
    }

}
