using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// 플레이어가 클리어 포탈에 도달했지만, 점수가 부족해서 클리어 할 수 없을때, 
///  안내 문구를 잠깐 표시하는 UI 관련 클래스.
/// </summary>

public class GoalRequirementUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private float showDuration = 2.0f;

    private Coroutine currentCoroutine = null;

    private void Start()
    {
        if (messageText != null)  // 메세지를 감추는 작업
        {
            messageText.gameObject.SetActive(false);
            // messageText.text = " ";
        }
    }

    public void ShowMessage(int requiredScore, int currentScore)
    {
        string message = "Need" + requiredScore.ToString() + "Score! Current : " + currentScore.ToString();

        messageText.text = message; // 넣어줌

        messageText.gameObject.SetActive (true); // 활성화

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null; 
        }


        // 코루틴 함수를 멈추기 위한 함수
        //StopCoroutine(HideMessage());
        //코루틴 함수를 사용하기 위한 함수
        currentCoroutine = StartCoroutine(HideMessage());
    }

    /// <summary>
    /// 코루틴 함수 
    /// </summary>
    /// <returns></returns>

    IEnumerator HideMessage()
    {
        // 2초 대기 후 메시지를 감춘다.
        yield return new WaitForSeconds(showDuration);

        // 다음 프레임 까지 대기하겠다.
        // yield return null;

        messageText.gameObject.SetActive(false );

        currentCoroutine = null;
    }

}
