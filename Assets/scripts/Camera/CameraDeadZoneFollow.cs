using Unity.Mathematics;
using UnityEngine;

public class CameraDeadZoneFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 카메라가 따라갈 대상 (Player)

    [SerializeField]
    private float deadzoneHalfWidth = 2.0f; // 좌우로 허용되는 범위.

    [SerializeField]
    private float deadzoneHalfHeight = 1.0f; // 상하로 허용되는 범위.

    [SerializeField]
    private float followSpeed = 6.0f; // 카메라가 따라가는 속도 (부드러움 정도)

    [SerializeField]
    private float cameraZ = -10.0f; // 카메라가 플레이어로 부터 z 좌표를 얼마나 떨어지게 할것인지.

    void LateUpdate()
    {
        if (target == null) // 타겟이 지정되지 않았다면 실행 중단.
        {
            return;
        }

        //타겟과 카메라간의 거리값 계산 (X, Y 계산)
        float diffX = target.position.x - transform.position.x;
        float diffY = target.position.y - transform.position.y;

        float targetCamX = transform.position.x;
        float targetCamY = transform.position.y;

        // - X축 : 좌우 체크 -
        // 플레이어 캐릭터가 데드존의 오른쪽을 벗어났다라는 의미
        if (diffX > deadzoneHalfWidth)
        {
            targetCamX = target.position.x - deadzoneHalfWidth; 
        }
        // 플레이어 캐릭터가 데드존의 왼쪽을 벗어났다라는 의미
        else if (diffX < -deadzoneHalfWidth)
        {
            targetCamX = target.position.x + deadzoneHalfWidth;
        }

        // - Y축 : 상하 체크 -
        // 플레이어 캐릭터가 데드존의 위쪽을 벗어났다라는 의미
        if (diffY > deadzoneHalfHeight)
        {
            targetCamY = target.position.y - deadzoneHalfHeight;
        }
        // 플레이어 캐릭터가 데드존의 아래쪽을 벗어났다라는 의미
        else if (diffY < -deadzoneHalfHeight)
        {
            targetCamY = target.position.y + deadzoneHalfHeight;
        }

        float lerpT = followSpeed * Time.deltaTime; // 얼마나 부드럽게 이동할지 결정하는 값 (속도 * 시간)

        // Mathf.Lerp(현재위치, 목표위치, 비율)를 사용해 부드럽게 따라가도록 계산합니다.
        //이동
        float newX = Mathf.Lerp(transform.position.x, targetCamX, lerpT);
        float newY = Mathf.Lerp(transform.position.y, targetCamY, lerpT);

        // 현재 카메라 위치 갱신 , 계산된 새 좌표를 카메라의 위치에 실제로 적용합니다. (Z값은 유지)
        transform.position = new Vector3 (newX, newY, cameraZ);
    }


}
