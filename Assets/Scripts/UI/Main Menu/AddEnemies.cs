using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddEnemies : MonoBehaviour
{
	public string character;
	public TMP_Text label;

	public void AddThisCharacter()
	{
		GlobalVariableStorage.CurrentEncounter.Add(character);
		Debug.Log(GlobalVariableStorage.GetEncounterAsText());
		label.text = GlobalVariableStorage.GetEncounterAsText();
	}

	public void RemoveThisCharacter()
	{
		GlobalVariableStorage.CurrentEncounter.Remove(character);
		Debug.Log(GlobalVariableStorage.GetEncounterAsText());
		label.text = GlobalVariableStorage.GetEncounterAsText();

	}
}
