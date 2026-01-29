using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    public static SceneTransition Instance; // 싱글톤 

    private void Awake()
    {
        Instance = this; // 클래스 자신을 저장

        // 씬이 파괴돼도 오브젝트가 파괴되지 않도록 해주는 함수. // ex) 사운드 및 데이터 저장때 유용하게 사용됨.
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextScene(string nextsceneName)
    {
        StartCoroutine(CoFadeOut(nextsceneName)); // 코루틴 함수 호출 방법
        
    }

    public void FadeIn()
    {
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        while (true)
        {
            Color color = fadeImage.color;
            color.a -= 0.005f;
            if (color.a <= 0.0f)
            {
                color.a = 0.0f;
                fadeImage.color = color;
                fadeImage.gameObject.SetActive(false);
                break;
            }
            fadeImage.color = color;
            yield return null;
        }
     
    }


    IEnumerator CoFadeOut(string nextSceneName) // 코루틴
    {
        fadeImage.gameObject.SetActive(true);

        while (true) // break 문쓰기 전까지는 무한 반복 처리.
        {
            Color color = fadeImage.color;
            color.a += 0.005f;
            if (color.a >= 1.0f)
            {
                color.a = 1.0f;
                fadeImage.color = color;
                break;
            }

            fadeImage.color = color;
            yield return null; // 코루틴 함수 쓸때 지정한 시간만큼 처리후 대기
        }

        SceneManager.LoadScene(nextSceneName);
        
    }
}
