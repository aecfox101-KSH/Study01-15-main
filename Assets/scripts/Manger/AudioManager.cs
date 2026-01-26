using UnityEngine;
public enum AudioType
{
    Jump = 0,
    GainCoin = 1
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSFX;

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

}
