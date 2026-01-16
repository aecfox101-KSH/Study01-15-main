using JetBrains.Annotations;
using UnityEngine;

public class playerStatus : MonoBehaviour  
{
    public string playerName = "me"; // 인스팩터 창에서 값을 설정해주는 것이 기준이기 때문에 굳이 이렇게 잡지 않아도 됨.
    public int  MaxHp = 50;
    public int Hp = 10;
    public int Attack = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Attack = 1000;
        Debug.Log("플레이어가 준비되었습니다." + playerName);
        Debug.Log(" HP" + Hp + "/" + MaxHp + "공격력: " + Attack);
    }
    // Update is called once per frame
    void Update() 
    {

    }
    /// <summary> 3개의 ///를 하면 주석이 나옴
    ///  플레이어가 대미지를 받아 HP 감소 처리.
    /// </summary>
    /// <param name="damage">대미지 양</param>
    public void TakeDamage(int damage) // 
    {
        Hp = Hp -   damage;

        if (Hp < 0)
        {
            Hp = 0;
        }

        Debug.Log(playerName + "이(가)" + damage + "의 대미지를 받았습니다. HP: " + Hp);

    }
    public void Heal(int amount)
    {
        Hp = Hp + amount;

        if (Hp > MaxHp)
        {
            Hp = MaxHp;
        }

        Debug.Log(playerName + "이(가)" + amount + "만큼 체력을 회복했습니다. HP: " + Hp);
    }

    public bool isDead() // 죽었는지 살았는지 확인하는 법
    {
        if (Hp <= 0)
        {
            return true;
        }
        /*else
        {
            return false;
        }*/
        return false;
    }
}
