using UnityEngine;
/// <summary>
/// 게임의 진행상태를 관리하는 매니저.
/// 현재 스테이지가 클리어 되었는지를 관리
/// </summary>


public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    private bool isStageClear = false;

    private void Awake()
    {
        instance = this;
    }

    public void SetStageClear()
    {
        isStageClear = true;
    }

    public bool IsStageClear()
    {
        return isStageClear;
    }

}
