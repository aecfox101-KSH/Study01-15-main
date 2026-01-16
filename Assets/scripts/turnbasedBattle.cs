using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class turnbasedBattle : MonoBehaviour 
{
    public playerStatus player;
    public MonsterStatus monster;

    public int TurnCount = 10;

    private void Start()
    {
        SimulateBattle();
    }

    void SimulateBattle()
    {
        for (int i = 1; i <= TurnCount; i++)
        {
            Debug.Log("== 턴 " + i + "시작!");

            Debug.Log("플레이어의 공격!");
            monster.Takedamage(player.Attack); // 몬스터의 체력을 깎아주는 함수 불러 옴

            if(monster.isDead() == true)
            {
                Debug.Log("몬스터를 처치했습니다. 플레이어 승리~");
                break;
            }

            Debug.Log("몬스터의 공격");

            player.TakeDamage(monster.Attack); // 몬스터의 공격력 함수 불러옴. 파라미터로!

            if (player.isDead() == true)
            {
                Debug.Log("플레이어를 처치했습니다. 몬스터 승리");
                break;
            }
            Debug.Log("===턴" + i + "종료");
        }
        Debug.Log("전투종료");
    }
}

