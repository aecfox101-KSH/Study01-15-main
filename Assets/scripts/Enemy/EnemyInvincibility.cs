using System.Collections;
using UnityEngine;

public class EnemyInvincibility : MonoBehaviour
{
    [SerializeField]
    private float InvincibilityDuration = 1.0f; // 무적 상태가 유지되는 총 시간

    [SerializeField]
    private float blinkInterval = 0.2f;// 캐릭터가 깜빡거리는 속도 (0.1초마다 켜졌다 꺼짐)

    [SerializeField]
    private SpriteRenderer spriteRenderer;// 캐릭터 이미지를 껐다 켰다 하기 위해 필요한 컴포넌트

    private bool isInvincible = false; // 현재 무적 상태인지 기록하는 스위치
    private Coroutine InvincibleCoroutine = null; // 현재 실행중인 코루틴을 기억해두는 변수

    // 적이 무적인지? 아닌지
    public bool IsInvincible() //무적 아니다 라면
    {
        return isInvincible;
    }

    // 무적상태 시작 함수
    public void StartInvincibility()
    {
        if(isInvincible == true) // 이미 무적이냐?
        {
            return;
        }

        if(spriteRenderer == null)
        {
            return;
        }
        if(InvincibleCoroutine != null) // 코루틴의 중복 방지
        {
            StopCoroutine(InvincibleCoroutine);
            InvincibleCoroutine = null;
        }

        InvincibleCoroutine = StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine() // 깜빡거림 or 시간 체크 동시 적용
    {
        isInvincible = true;

        float elapsed = 0.0f;

        while(elapsed < InvincibilityDuration)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2.0f;
        }

        spriteRenderer.enabled = true;

        isInvincible = false;

        InvincibleCoroutine = null;
    }
}
