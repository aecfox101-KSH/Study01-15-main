using UnityEngine;
using UnityEngine.UI;

public class HorizontalScroll : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopUIPanel;

    [SerializeField]
    private Transform content;

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private int cardCount = 10;

    private int cardIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < cardCount; i++)
        {
            AddCard();
        }

        ShopUIPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B) ==  true)
        {
            ShopUIPanel.SetActive(!ShopUIPanel.activeSelf);
        }
    }

    void AddCard()
    {
        if (content == null)
        {
            return;
        }

        if (cardPrefab == null)
        {
            return; 
        }

        GameObject go = Instantiate(cardPrefab, content);

        if (go != null)
        {
            cardIndex++;
            Debug.Log(cardIndex + "번째 카드 생성 성공!!");
        }
    }
}
