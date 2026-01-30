using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickTitleScreen()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수를 등록.
            SceneTransition.Instance.FadeOutEvent += LoadtoLoadingScene;
            SceneTransition.Instance.StartFadeOut();

        }
    }

    void OnDisable()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수를 등록 해제
            SceneTransition.Instance.FadeOutEvent -= LoadtoLoadingScene;
        }
    }

    void OnDestroy()
    {
        if (SceneTransition.Instance != null)
        {
            // 이벤트 함수를 등록 해제
            SceneTransition.Instance.FadeOutEvent -= LoadtoLoadingScene;
        }
    }

    void LoadtoLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
