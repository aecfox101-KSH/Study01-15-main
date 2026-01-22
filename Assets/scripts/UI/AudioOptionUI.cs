using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SaveType
{
    PlayerPrefs = 0,
    Json = 1,
    Binary =2,
}

public class AudioOptionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private string mainVolumeParam = "MainVolume";

    [SerializeField]
    private string bgmVolumeParam = "BGMVolume";

    [SerializeField]
    private string sfxVolumeParam = "SFXVolume";

    [SerializeField]
    private Slider mainSlider;

    [SerializeField]
    private Slider bgmSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private string mainKey = "Opt_MainVolume01";

    [SerializeField]
    private string bgmKey = "Opt_BgmVolume01";

    [SerializeField]
    private string sfxKey = "Opt_SfxVolume01";

    [SerializeField]
    private float defaultMain = 1.0f;

    [SerializeField]
    private float defaultBgm = 1.0f;

    [SerializeField]
    private float defaultSfx = 1.0f;

    [SerializeField]
    private SaveType saveType = SaveType.PlayerPrefs;

    private bool isInitializing = false;

    private OptionData data = null;
    private OptionJsonStorage Storage = null;
    private OptionsBinaryStorage storageBin = null;

    private void Start()
    {
        Storage = new OptionJsonStorage();
        storageBin = new OptionsBinaryStorage();
        optionPanel.SetActive(false);
        Load();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }
    }
    public void OnMainSlider(float value)
    {
        ApplyMixerVolume(mainVolumeParam, value);

        switch (saveType)
        {
            case SaveType.Json:
                {
                    data.mainVolume01 = value;
                    Storage.Save(data);
                }

                break;

            case SaveType.Binary:
                {
                    data.mainVolume01 = value;
                    storageBin.Save(data);
                }
                break;

            default:
                {
                    PlayerPrefs.SetFloat(mainKey, value); 
                }
                 break;
        }
    }

    public void OnBGMSlider(float value)
    {
        ApplyMixerVolume(bgmVolumeParam, value);

        switch (saveType)
        {
            case SaveType.Json:
                {
                    data.bgmVolume01 = value;
                    Storage.Save(data);
                }
                break;

            case SaveType.Binary:
                {
                    data.bgmVolume01 = value;
                    storageBin.Save(data);
                }
                break;

            default:
                {
                   PlayerPrefs.SetFloat(bgmKey, value);
                }
                break;
        }
    }

    public void OnSFXSlider(float value)
    {
        ApplyMixerVolume(sfxVolumeParam, value);

        switch(saveType)
        {
            case SaveType.Json:
                {
                    data.sfxVolume01 = value;
                    Storage.Save(data);
                }
                break;

            case SaveType.Binary:
                {
                    data.sfxVolume01 = value;
                    storageBin.Save(data);
                }
                break;

            default:
                {
                   PlayerPrefs.SetFloat(sfxKey, value);
                }
                break;
        }
    }

    void ApplyMixerVolume(string paramName, float value)
    {
        // value는 0~1 사이의 값.
        // 이 값을 그대로 AudioMixer에 적용하면 안된다.
        // AudioMixer의 볼륨 최소/최대 범위의 값은 -80 ~ 0
        // value를 -80 ~ 0 사이 의 값으로 환산하는 과정을 거쳐야 함. (-80 ~ 0 값은 규칙 같은 느낌임)

        float safeValue = value;
        if(safeValue <= 0.0f)
        {
            // 아래의 Log10 함수를 사용할 때 문제가 되지 않도록 값을 보정.
            safeValue = 0.0001f;
        }
        // 파라미터로 받은 볼륨을 - 80 ~ 0 사이의 값으로 환산.
        float db = Mathf.Log10(safeValue) * 20.0f;

        mixer.SetFloat(paramName, db);
    }

    public void Load()
    {
        isInitializing = true;

        float main01 = 0.0f;
        float bgm01 = 0.0f;
        float sfx01 = 0.0f;


        switch(saveType)
        {
            case SaveType.Json:
                {
                    data = Storage.Load();

                    // 데이터를 불러오지 못했을 경우 기본 값으로 세팅해준다.
                    if (data == null)
                    {
                        data = new OptionData();
                        data.mainVolume01 = defaultMain;
                        data.bgmVolume01 = defaultBgm;
                        data.sfxVolume01 = defaultSfx;
                    }

                    main01 = data.mainVolume01;
                    bgm01 = data.bgmVolume01;
                    sfx01 = data.sfxVolume01;
                }
                break;

            case SaveType.Binary:
                {
                    data = storageBin.Load();
                    // 데이터를 불러오지 못했을 경우 기본 값으로 세팅해준다.
                    if (data == null)
                    {
                        data = new OptionData();
                        data.mainVolume01 = defaultMain;
                        data.bgmVolume01 = defaultBgm;
                        data.sfxVolume01 = defaultSfx;
                    }
                    main01 = data.mainVolume01;
                    bgm01 = data.bgmVolume01;
                    sfx01 = data.sfxVolume01;
                }
                break;

            default:
                {
                    // 기기에 저장되어 있는 메인 볼륨 값 데이터를 불러온다.
                    // 만약 불러올 데이터가 없으면 기본 값으로 defaultMain을 대입한다.
                    main01 = PlayerPrefs.GetFloat(mainKey, defaultMain);
                    bgm01 = PlayerPrefs.GetFloat(bgmKey, defaultBgm);
                    sfx01 = PlayerPrefs.GetFloat(sfxKey, defaultSfx);
                }
                break;
        }
            mainSlider.value = main01;
            bgmSlider.value = bgm01;
            sfxSlider.value = sfx01;

            ApplyMixerVolume(mainVolumeParam, main01);
            ApplyMixerVolume(bgmVolumeParam, bgm01);
            ApplyMixerVolume(sfxVolumeParam, sfx01);

            isInitializing = false;
    }
}
