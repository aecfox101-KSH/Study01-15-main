using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    // [기존 수정] 특정 이름("ksh")으로 고정하지 않기 위해 gameSceneName은 더 이상 직접 쓰지 않습니다.
    [SerializeField]
    private string gameSceneName = "ksh";

    [SerializeField]
    private TMP_Text loadingText;

    [SerializeField]
    private Image progressFillImage;

    private AsyncOperation op;

    // [추가] 페이드 아웃 이벤트가 여러 번 중복 호출되는 것을 막기 위한 변수입니다.
    private bool isTransitionStarted = false;

    void Start()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.FadeInEvent += StartLoadGameScenc;
            SceneTransition.Instance.FadeIn();
        }
    }

    void OnDisable()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.FadeInEvent -= StartLoadGameScenc;
            SceneTransition.Instance.FadeOutEvent -= LoadToGameScene;
        }
    }

    void OnDestroy()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.FadeInEvent -= StartLoadGameScenc;
            SceneTransition.Instance.FadeOutEvent -= LoadToGameScene;
        }
    }

    void StartLoadGameScenc()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        // ================= [여기서부터 수정 및 추가된 로직] =================

        // 1. PlayerPrefs에서 저장된 스테이지 번호를 읽어옵니다. (기본값 1)
        int savedStage = PlayerPrefs.GetInt("SavedStage", 1);
        string actualSceneName;

        // 2. 씬 이름 규칙을 설정합니다. (1번은 ksh, 2번부터는 ksh2, ksh3...)
        if (savedStage == 1)
        {
            actualSceneName = "ksh";
        }
        else
        {
            actualSceneName = "ksh" + savedStage;
        }

        Debug.Log("로딩할 대상 씬: " + actualSceneName);

        // 3. 중요: gameSceneName 대신 실제 이름인 'actualSceneName'을 로드합니다.
        op = SceneManager.LoadSceneAsync(actualSceneName);

        // =================================================================

        op.allowSceneActivation = false;

        while (op.isDone == false)
        {
            float raw = op.progress;
            float normalized = Mathf.Clamp01(raw / 0.9f);

            if (progressFillImage != null)
            {
                progressFillImage.fillAmount = normalized;
            }

            if (loadingText != null)
            {
                int percent = Mathf.RoundToInt(normalized * 100.0f);
                loadingText.text = "로딩중..." + percent.ToString() + "%";
            }

            // [수정] raw가 0.9f에 도달하고 아직 전환이 시작되지 않았을 때만 실행
            if (raw >= 0.9f && !isTransitionStarted)
            {
                isTransitionStarted = true; // [추가] 이제 페이드 아웃을 한 번만 실행하도록 잠금

                if (SceneTransition.Instance != null)
                {
                    SceneTransition.Instance.FadeOutEvent += LoadToGameScene;
                    SceneTransition.Instance.StartFadeOut();

                    // [추가] 코루틴이 중복으로 돌지 않도록 여기서 중단합니다.
                    yield break;
                }
            }

            yield return null;
        }
    }

    void LoadToGameScene()
    {
        // [추가] 로딩이 멈춘 것인지 확인하기 위한 디버그 로그
        Debug.Log("씬 전환 스위치 on: " + PlayerPrefs.GetInt("SavedStage") + "스테이지로 진입합니다.");
        op.allowSceneActivation = true;
    }
}