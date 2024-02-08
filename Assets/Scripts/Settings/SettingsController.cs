using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Screen resolution")]
    public TMP_Dropdown resolutionOptions;
    private bool isFullScreen;
	private int resWidth, resHeight;

	[Header("Frame Rate")]
	public TMP_Dropdown framerateOptions;
	public int framerate;

	private void Start()
	{
		//Make it so the option sends when the player changes the resolution
		resolutionOptions.onValueChanged.AddListener(delegate { ChangeResolution(); });

		framerateOptions.onValueChanged.AddListener(delegate { ChangeFrameRate(); });
	}

	public void ChangeResolution()
	{
        string[] resolution = resolutionOptions.options[resolutionOptions.value].text.Split('x');
		int.TryParse(resolution[0], out resWidth);
		int.TryParse(resolution[1], out resHeight);
		Screen.SetResolution(resWidth, resHeight, isFullScreen);
	}

	public void ChangeFrameRate()
	{
		int.TryParse(framerateOptions.options[framerateOptions.value].text, out framerate);
		Application.targetFrameRate = framerate;
		Debug.Log("[SettingsContoller][ChangeFramerate]: Current Trarget framerate is " + Application.targetFrameRate);
	}

	public void ToggleFullScreen()
	{
        isFullScreen = !isFullScreen;
	}
}
