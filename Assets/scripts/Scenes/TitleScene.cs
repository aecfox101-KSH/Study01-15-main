using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 버튼 컴포넌트 접근을 위해 추가

public class TitleScene : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private Button continueButton; // 이어하기 버튼을 인스펙터에서 연결하세요

    void Start()
    {
        // [추가] 저장된 데이터가 없으면 이어하기 버튼을 비활성화(클릭 불가) 처리
        if (continueButton != null)
        {
            // "SavedStage" 키가 있는지 확인하여 버튼 활성화 여부 결정
            continueButton.interactable = PlayerPrefs.HasKey("SavedStage");
        }
    }

    // [수정/추가] 새 게임 시작 버튼에 연결할 함수
    public void OnClickNewGame()
    {
        // 1. 기존 진행 데이터를 1단계로 강제 초기화
        PlayerPrefs.SetInt("SavedStage", 1);
        PlayerPrefs.Save(); // 데이터 물리적 저장

        Debug.Log("새 게임을 시작합니다. 스테이지 1로 초기화되었습니다.");

        // 2. 페이드 아웃 연출 시작
        StartTransition();
    }

    // [추가] 이어하기 버튼에 연결할 함수
    public void OnClickContinue()
    {
        // 1. 저장된 데이터가 있는지 한 번 더 체크 (보통 interactable로 막히지만 안전을 위해)
        if (PlayerPrefs.HasKey("SavedStage"))
        {
            int saved = PlayerPrefs.GetInt("SavedStage");
            Debug.Log("이어서 시작합니다. 현재 저장된 스테이지: " + saved);
        }
        else
        {
            // 혹시 데이터가 없는데 눌렸다면 1번으로 세팅
            PlayerPrefs.SetInt("SavedStage", 1);
        }

        // 2. 페이드 아웃 연출 시작
        StartTransition();
    }

    // [중복 로직 분리] 공통 페이드 아웃 시작 함수
    private void StartTransition()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수 등록
            SceneTransition.Instance.FadeOutEvent += LoadtoLoadingScene;
            SceneTransition.Instance.StartFadeOut();
        }
    }

    void OnDisable()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수 등록 해제
            SceneTransition.Instance.FadeOutEvent -= LoadtoLoadingScene;
        }
    }

    void OnDestroy()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수 등록 해제
            SceneTransition.Instance.FadeOutEvent -= LoadtoLoadingScene;
        }
    }

    void LoadtoLoadingScene()
    {
        // 로딩 씬으로 이동 (이후 로딩 씬이 PlayerPrefs를 읽어 ksh 또는 ksh2를 로드함)
        SceneManager.LoadScene("LoadingScene");
    }
}
