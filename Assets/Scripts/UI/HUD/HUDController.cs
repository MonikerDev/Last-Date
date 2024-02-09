using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("HUD Elements")]
    public TMP_Text locationLabel;
    public TMP_Text dayLabel;
    public TMP_Text timeLabel;
    public Slider stamBar;

    //Time converters
    private int hour;
    private int minutes;

    [Header("Player data")]
    public PhysicsBasedPlayerController player;

	private void Start()
	{
        SetupStamBar();
        UpdateLocation();
        UpdateTime();
        UpdateDay();
	}

	private void Update()
	{
        UpdateStamBar();
	}

	public void UpdateLocation()
	{
        locationLabel.text = GlobalVariableStorage.location;
	}

    public void UpdateTime()
	{
        hour = GlobalVariableStorage.time / 60;
        minutes = GlobalVariableStorage.time % 60;

        timeLabel.text = hour + ":" + minutes;
	}

    public void UpdateDay()
	{
        dayLabel.text = GlobalVariableStorage.day;
	}

    public void SetupStamBar()
	{
        stamBar.maxValue = player.maxSprintEnergy;
        stamBar.minValue = 0;
        stamBar.value = stamBar.maxValue;
	}

    public void UpdateStamBar()
	{
        stamBar.value = player.currSprintEnergy;
	}
}
