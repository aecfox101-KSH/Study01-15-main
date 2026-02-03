using UnityEngine;
using System.Collections;

public class ChapterUIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject chapter1Panel; // 챕터 UI 패널
    [SerializeField] private float displayTime = 2.5f; // 보여줄 시간
    
    

    void Start()
    {
        if (AudioManager.instance != null)
        {
            // Enum에 추가한 BGM 인덱스를 넣으세요
            AudioManager.instance.PlayBGM(AudioType.BGM_Main);
        }
        // 씬이 시작되자마자 연출 시작
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // 1. 최신 방식으로 플레이어 오브젝트 찾기
        // (주의: PlayerContraller 클래스명 오타 확인!)
        PlayerContraller player = Object.FindAnyObjectByType<PlayerContraller>();

        // 2. 조작 차단
        if (player != null)
        {
            player.canControl = false;
        }

        // 3. 챕터 패널 활성화
        if (chapter1Panel != null)
        {
            chapter1Panel.SetActive(true);
        }

        // 4. 연출 대기
        yield return new WaitForSeconds(displayTime);

        // 5. 패널 비활성화 및 조작 복구
        if (chapter1Panel != null)
        {
            chapter1Panel.SetActive(false);
        }

        if (player != null)
        {
            player.canControl = true;
        }

        Debug.Log("챕터 소개 종료, 게임 시작!");
    }
}


