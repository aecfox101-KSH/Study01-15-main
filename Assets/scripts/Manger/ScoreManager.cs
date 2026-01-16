using UnityEngine;

/// <summary>
/// 점수를 관리하는 매니저 스크립트
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // 싱글톤(Singleton) : 어디서든 ScoreManager에 접근하기 위한 정적(static) 변수 / 정적 - 고정되어 있다 정도로 알고 있자.
    // 현재는 비어 있는 상태.
    public static ScoreManager instance;

    private int currentScore = 0;

    void Awake()
    {
        // 클래스 자신으로 초기화하여 외부에서 사용할 수 있게 해줌.
        instance = this; // this = 클래스 자신을 의미함.
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 점수 증가 시키는 함수
    /// </summary>
    /// <param name="amount">증가시킬 점수</param>
    public void AddScore(int amount)
    {
        currentScore += amount;
    }

    /// <summary>
    /// 현재 점수를 반환.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return currentScore;
    }
    

}
