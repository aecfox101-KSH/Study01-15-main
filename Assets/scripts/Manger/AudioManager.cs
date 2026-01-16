using UnityEngine;

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

    public void  PlaySFX(int index)
    {
        if (index <  0 || index >= clips.Length)
        {
            return;
        }
        audioSFX.clip = clips[index];
        audioSFX.Play();
    }

}
