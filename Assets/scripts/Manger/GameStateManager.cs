using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 게임의 진행상태를 관리하는 매니저.
/// 현재 스테이지가 클리어 되었는지를 관리
/// </summary>

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    [Header("설정")]
    public int currentStage; // 현재 스테이지 번호
    private bool isStageClear = false;

    private void Awake()
    {
        instance = this;

        if(SceneTransition.Instance != null )
        {
            SceneTransition.Instance.FadeIn();
        }
    }

    // GameStateManager.cs 안에 추가
    private void Start()
    {
        // 예: 스테이지 1이면 0번 음악, 스테이지 2면 1번 음악...
        // 인덱스는 본인의 AudioType 설정에 맞게 조절하세요!
        if (currentStage == 1) AudioManager.instance.PlayBGM(AudioType.BGM_Main);
        else if (currentStage == 2) AudioManager.instance.PlayBGM(AudioType.BGM_Stage2);
    }

    public void SetStageClear()
    {
        isStageClear = true;
        // 클리어 정보를 PlayerPreFs에 즉시 저장
        PlayerPrefs.SetInt("SavedStage", currentStage + 1);
        PlayerPrefs.Save();
    }

    public void LoadNextStage()
    {
        // 1. 현재 스테이지 번호를 가져옴 (PlayerPrefs에 저장된 값을 활용)
        int savedStage = PlayerPrefs.GetInt("SavedStage", 1);

        // 2. 만약 마지막 스테이지를 넘겼다면 엔딩으로, 아니면 로딩씬으로!
        // 예: 스테이지가 3개라면 4가 되었을 때 엔딩
        if (savedStage > 3)
        {
            SceneManager.LoadScene("EndingScene");
        }
        else
        {
            // 로딩 씬으로 이동 (로딩 씬이 PlayerPrefs를 보고 다음 스테이지를 로드함)
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public bool IsStageClear()
    {
        return isStageClear;
    }

}
