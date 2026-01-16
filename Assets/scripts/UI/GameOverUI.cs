using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOverUI : MonoBehaviour
{
    public void OnClickRestart()
    {
        // SceneManager.LoadScene("Untitled");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}