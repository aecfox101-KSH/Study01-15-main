using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public string MonsterName;
    public int MaxHp;
    public int HP;
    public int Attack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("몬스터가 준비 되었습니다." +  MonsterName);
        Debug.Log("HP :  " +  HP + " / " + "MaxHp : " + MaxHp + "공격력 : " + Attack);
    }
    /// <summary>
    /// 1. 대미지 준것
    /// 2. 회복
    /// 3. 죽었는지 살았는지 확인.
    /// </summary>
    // Update is called once per frame
    public void Takedamage(int damage)
    {
        HP = HP - damage;
        if (HP < 0)
        {
            HP = 0;
        }
        Debug.Log(MonsterName + "이(가)" + damage + "를 플레이어 에게 받았습니다. HP = " + HP);
    }
    public void Healing(int amount)
    {
        HP = HP + amount;
        if (HP > MaxHp)
        {
            HP = MaxHp;
        }
    }
    public bool isDead()
    {
        if (HP <= 0)
        {
            return true;
        }
        return false;
    }
}
