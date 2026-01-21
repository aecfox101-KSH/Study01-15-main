using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// 저장 방식을 선택하기 위한 열거형 데이터
public enum SaveType
{
    PlayerPrefs = 0,
    Json = 1,
    Binary = 2,
}

public class TestAudioOptionUI : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel; // 옵션 UI 패널
    [SerializeField] private AudioMixer mixer;       // 제어할 오디오 믹서

    [Header("Mixer Parameters")]
    [SerializeField] private string mainVolumeParam = "MainVolume";
    [SerializeField] private string bgmVolumeParam = "BGMVolume";
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    [Header("Sliders")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("PlayerPrefs Keys")]
    [SerializeField] private string mainKey = "Opt_MainVolume01";
    [SerializeField] private string bgmKey = "Opt_BgmVolume01";
    [SerializeField] private string sfxKey = "Opt_SfxVolume01";

    [Header("Default Values")]
    [SerializeField] private float defaultMain = 1.0f;
    [SerializeField] private float defaultBgm = 1.0f;
    [SerializeField] private float defaultSfx = 1.0f;

    [SerializeField] private SaveType saveType = SaveType.PlayerPrefs; // 인스펙터에서 설정 가능

    private bool isInitializing = false; // 로드 중 저장 방지용 플래그

    private OptionData data = new OptionData(); // 데이터 객체 (Null 방지를 위해 미리 생성)
    private OptionJsonStorage Storage = null;
    private OptionsBinaryStorage storageBin = null;

    private void Start()
    {
        // 저장 장치 객체 생성
        Storage = new OptionJsonStorage();
        storageBin = new OptionsBinaryStorage();

        optionPanel.SetActive(false);
        Load(); // 게임 시작 시 데이터 로드
    }

    private void Update()
    {
        // ESC 키로 옵션창 토글
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
        }
    }

    // --- 슬라이더 조작 시 실행되는 함수들 ---

    public void OnMainSlider(float value)
    {
        ApplyMixerVolume(mainVolumeParam, value);
        data.mainVolume01 = value;
        Save(); // 값 변경 시 즉시 저장
    }

    public void OnBGMSlider(float value)
    {
        ApplyMixerVolume(bgmVolumeParam, value);
        data.bgmVolume01 = value;
        Save();
    }

    public void OnSFXSlider(float value)
    {
        ApplyMixerVolume(sfxVolumeParam, value);
        data.sfxVolume01 = value;
        Save();
    }

    /// <summary>
    /// 실제 오디오 믹서의 볼륨을 조절하는 함수 (0.0001 ~ 1.0 값을 -80 ~ 0db로 변환)
    /// </summary>
    void ApplyMixerVolume(string paramName, float value)
    {
        float safeValue = value;
        if (safeValue <= 0.0f) safeValue = 0.0001f; // 로그 계산 오류 방지

        float db = Mathf.Log10(safeValue) * 20.0f;
        mixer.SetFloat(paramName, db);
    }

    // --- 미션 1: Save 로직을 switch ~ case로 변경 ---
    private void Save()
    {
        // 데이터가 불러와지는 중(Load 메서드 실행 중)에는 저장을 건너뜀
        if (isInitializing) 
        {
            return; 
        }

        switch (saveType)
        {
            case SaveType.Json:
                Storage.Save(data);
                break;

            case SaveType.Binary:
                storageBin.Save(data);
                break;

            case SaveType.PlayerPrefs:
                // PlayerPrefs는 각각의 키값으로 개별 저장
                PlayerPrefs.SetFloat(mainKey, data.mainVolume01);
                PlayerPrefs.SetFloat(bgmKey, data.bgmVolume01);
                PlayerPrefs.SetFloat(sfxKey, data.sfxVolume01);
                PlayerPrefs.Save(); // 디스크에 즉시 기록
                break;
        }
    }

    // --- 미션 1: Load 로직을 switch ~ case로 변경 ---
    public void Load()
    {
        // 슬라이더 값을 세팅할 때 OnSlider 이벤트가 발생하여 Save()가 중복 호출되는 것을 막기 위함
        isInitializing = true;

        switch (saveType)
        {
            case SaveType.Json:
                // JSON 파일로부터 데이터를 읽어옴
                OptionData loadedJson = Storage.Load();
                if (loadedJson != null)
                    data = loadedJson;
                else
                    SetDefaultData(); // 로드 실패 시 기본값 세팅
                break;

            case SaveType.Binary:
                // Binary 파일로부터 데이터를 읽어옴
                OptionData loadedBin = storageBin.Load();
                if (loadedBin != null)
                    data = loadedBin;
                else
                    SetDefaultData();
                break;

            case SaveType.PlayerPrefs:
            default:
                // 기기 내장 저장소에서 각각 읽어옴
                data.mainVolume01 = PlayerPrefs.GetFloat(mainKey, defaultMain);
                data.bgmVolume01 = PlayerPrefs.GetFloat(bgmKey, defaultBgm);
                data.sfxVolume01 = PlayerPrefs.GetFloat(sfxKey, defaultSfx);
                break;
        }

        // 로드된 데이터를 UI 슬라이더에 적용
        mainSlider.value = data.mainVolume01;
        bgmSlider.value = data.bgmVolume01;
        sfxSlider.value = data.sfxVolume01;

        // 실제 소리 크기 적용
        ApplyMixerVolume(mainVolumeParam, data.mainVolume01);
        ApplyMixerVolume(bgmVolumeParam, data.bgmVolume01);
        ApplyMixerVolume(sfxVolumeParam, data.sfxVolume01);

        // 로드 완료 후 다시 저장 가능 상태로 전환
        isInitializing = false;
    }

    /// <summary>
    /// 저장된 데이터가 없을 경우 기본값으로 초기화하는 헬퍼 함수
    /// </summary>
    private void SetDefaultData()
    {
        data.mainVolume01 = defaultMain;
        data.bgmVolume01 = defaultBgm;
        data.sfxVolume01 = defaultSfx;
    }
}