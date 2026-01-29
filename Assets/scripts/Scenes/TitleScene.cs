using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public void OnClickTitleScreen()
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.LoadNextScene("ksh");
        }
    }
}
