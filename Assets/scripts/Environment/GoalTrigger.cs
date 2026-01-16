using Unity.VisualScripting;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private int requiredScore = 10;

    [SerializeField]
    private int clearBonusScore = 20;

    [SerializeField]
    private GoalRequirementUI requirementUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
        {
            return;
        }

        if(GameStateManager.instance == null)
        {
            return;
        }

        int currentScore = ScoreManager.instance.GetScore();

        if (currentScore >= requiredScore)
        {
            GameStateManager.instance.SetStageClear();

            ScoreManager.instance.AddScore(clearBonusScore);
        }
        else
        {
            if(requirementUI !=null)
            {
                requirementUI.ShowMessage(requiredScore,currentScore); 
            }
        }

    }

}
