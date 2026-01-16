using UnityEngine;

public class StageClearUI : MonoBehaviour
{
	[SerializeField]
	private GameObject stageClearPanel;

	private bool clearShown = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		Time.timeScale = 1.0f;
		if (stageClearPanel != null)
		{
			stageClearPanel.SetActive(false);
		}

		clearShown = false;

	}
	// Update is called once per frame
	void Update()
	{
		if (clearShown == true)
		{
			return;

		}

		if(GameStateManager.instance == null)
		{
			return ;
		}

		if(GameStateManager.instance.IsStageClear() == true)
		{
			stageClearPanel.SetActive(true);
			clearShown = true;
			Time.timeScale = 0.0f;
		}
	}
}
