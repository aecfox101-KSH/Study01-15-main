using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.instance == null)
        {
            return; 
        }

		// 싱글톤을 사용해서 현재 점수 정보를 가져온다.
		int score = ScoreManager.instance.GetScore(); // 싱글톤 사용법

        string textValue = "Score : " + score.ToString();  // int 형을 문자열로 바꿔주기 위해 ToString 사용
        scoreText.text = textValue;
    }
}
