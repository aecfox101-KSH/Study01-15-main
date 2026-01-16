using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어가 데미지를 입었을 때 잠시 동안 무적 상태로 만들고 캐릭터를 깜빡이게 함
public class PlayerInvincibility : MonoBehaviour
{
    [SerializeField]
    private float InvincibilityDuration = 1.0f; // 무적 상태가 유지되는 총 시간 (예: 1초)

    [SerializeField]
    private float blinkInterval = 0.1f; // 캐릭터가 깜빡거리는 속도 (0.1초마다 켜졌다 꺼짐)

    [SerializeField]
    private SpriteRenderer spriteRenderer; // 캐릭터 이미지를 껐다 켰다 하기 위해 필요한 컴포넌트

    private bool isInvincible = false; // 현재 무적 상태인지 기록하는 스위치

    private Coroutine InvincibleCoroutine = null; // 현재 실행 중인 코루틴을 기억해두는 변수

    // 외부(예: EnemyContactDamage)에서 플레이어가 무적인지 물어볼 때 답해주는 함수
    public bool IsInvincible()
    {
        return isInvincible;
    }

    // 무적 상태를 시작하라고 명령하는 함수
    public void StartInvincibility()
    {
        // 이미 무적 상태라면 다시 시작하지 않고 그냥 돌아갑니다.
        if (isInvincible == true)
        {
            return;
        }
        // 캐릭터 이미지(SpriteRenderer)가 연결되어 있지 않으면 에러 방지를 위해 종료합니다.
        if (spriteRenderer == null)
        {
            return;
        }
        // 혹시 이미 돌아가고 있는 무적 코루틴이 있다면 강제로 멈춥니다. (중복 방지)
        if (InvincibleCoroutine != null)
        {
            StopCoroutine(InvincibleCoroutine); // 오타 수정: 강사님이 알려준 방식 - lnvincibilityCoroutine()  -> InvincibleCoroutine
            InvincibleCoroutine = null;
        }
        // 무적 코루틴(깜빡거림)을 실제로 시작합니다.
        InvincibleCoroutine = StartCoroutine(lnvincibilityCoroutine());

    }

    // [핵심] 시간을 다루는 특수 함수(코루틴) : 깜빡거림과 시간 체크를 동시에 합니다.
    private IEnumerator lnvincibilityCoroutine()
    {
        isInvincible = true; // 무적 스위치 ON!
         
        float elapsed = 0.0f; // 시간이 얼마나 흘렀는지 잴 변수

        while (elapsed < InvincibilityDuration) // 무적 지속 시간(Duration)이 다 될 때까지 반복합니다.
        {
            spriteRenderer.enabled = false; // 1. 이미지를 끕니다.
            yield return new WaitForSeconds(blinkInterval); // 2. 설정한 간격(blinkInterval)만큼 잠시 기다립니다.

            spriteRenderer.enabled=true; // 3. 이미지를 다시 켭니다.
            yield return new WaitForSeconds(blinkInterval); // 4. 또 잠시 기다립니다.

            elapsed += blinkInterval * 2.0f; // 5. 방금 기다린 시간(꺼진 시간 + 켜진 시간)만큼 흐른 시간에 더해줍니다.
        }

        spriteRenderer.enabled = true; // 반복문이 끝나면(무적 시간이 종료되면) 이미지를 확실히 켜줍니다.

        isInvincible = false; // 무적 스위치 OFF! 이제 다시 데미지를 입을 수 있습니다.

        InvincibleCoroutine = null; // 코루틴 변수를 비워서 끝났음을 알립니다.
    }
}
