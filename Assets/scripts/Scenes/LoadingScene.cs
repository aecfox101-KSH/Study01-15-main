using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private string gameSceneName = "ksh";

    [SerializeField]
    private TMP_Text loadingText;

    [SerializeField]
    private Image progressFillImage;

    private AsyncOperation op;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            // 이벤트 함수를 등록 해제
            SceneTransition.Instance.FadeInEvent -= StartLoadGameScenc;
            SceneTransition.Instance.FadeOutEvent -= LoadToGameScene;

        }
    }

    void OnDestroy()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수를 등록 해제
            SceneTransition.Instance.FadeInEvent -= StartLoadGameScenc;
            SceneTransition.Instance.FadeOutEvent -= LoadToGameScene;

        }
    }


    void StartLoadGameScenc()
    {
        // 코루틴 함수를 호출해서 비동기로 다음 씬을 로딩하고, 로딩 진행률을 UI에 표시한다.
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        // 씬이 로딩이 되고있는 동안 다른 씬 또한 로딩이 필요한 경우 사용 LoadSceneAsync
        op = SceneManager.LoadSceneAsync(gameSceneName); 
        
        // 로딩이 끝났을 때 바로 씬 전환을 할 지 여부
        op.allowSceneActivation = false; 
        
        // isDone : 씬 로딩이 끝났는지 여부.
        while (op.isDone == false)
        {
            float raw = op.progress; // 0.0f ~ 0.9f 진행률

            // Clamp01 : 파라미터로 전달한 값이 0 ~ 1 사이의 범위를 벗어나지 않게 보정해주는 함수.
            float normalized = Mathf.Clamp01(raw/0.9f); // raw 값이 0.0f ~ 1.0f로 환산

            if(progressFillImage != null)
            {
                progressFillImage.fillAmount = normalized;
            }

            if (loadingText != null)
            {
                // RoundToInt : 파라미터로 전달된 값을 반올림 해서 정수형 값으로 반환해주는 함수.
                int percent = Mathf.RoundToInt(normalized * 100.0f);
                loadingText.text = "로딩중..." + percent.ToString() + "%";
            }

            // 조건이 만족하면 로딩이 완료된 것으로 간주
            if (raw >= 0.9f)
            {
                if (SceneTransition.Instance != null)
                {
                    // 개선의 여지가 있는 코드.
                    // 게임 씬 뿐만 아니라 어느 씬이든 이동이 가능하도록 코드를 개선할 필요가 있다.
                    SceneTransition.Instance.FadeOutEvent += LoadToGameScene;
                    SceneTransition.Instance.StartFadeOut();
                }
            }

            yield return null; // 매프레임 마다 진행률 갱신할 수 있도록
        }
    }

    void LoadToGameScene()
    {
        op.allowSceneActivation = true;
    }
}
