using UnityEngine;
using TMPro;

public class CoinCountText : MonoBehaviour
{
    [SerializeField] private TMP_Text texCoinCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texCoinCount.text = "CoinCount";
        int aaa = 1;
        texCoinCount.text = aaa.ToString(); //숫자를 문자열로 바꿔줌
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
