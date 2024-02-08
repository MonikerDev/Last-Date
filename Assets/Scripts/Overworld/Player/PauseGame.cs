using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [Header("Input Key")]
    KeyCode pauseKey = KeyCode.Escape;

    [Header("Menu")]
    public GameObject pauseUI;
    bool uiOpen;

	private void Start()
	{
		uiOpen = false;
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(pauseKey) && !uiOpen)
		{
			pauseUI.SetActive(true);
			uiOpen = true;
			PhysicsBasedPlayerController.canMove = false;
		}
		else if (Input.GetKeyDown(pauseKey) && uiOpen)
		{
			pauseUI.SetActive(false);
			uiOpen = false;
			PhysicsBasedPlayerController.canMove = true;
		}
    }
}
