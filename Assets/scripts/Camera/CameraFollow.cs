using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0.0f, 0.0f, -10.0f);

    /// <summary>
    /// Update() 함수가 실행된 직후에 호출, 주로 카메라 이동 and 캐릭터를 따라다니는 오브젝트 로직임.
    /// </summary>
    void LateUpdate() 
    {
        if (target == null) // null = 아무것도 들어가 있지 않는 상태 의미
        {
            return;
        }

        Vector3 targetposition = target.position + offset;

        transform.position = targetposition;
    }
}
