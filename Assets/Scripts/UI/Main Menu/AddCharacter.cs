using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCharacter : MonoBehaviour
{
	public TMP_Text label;
	public string character;
	public void AddThisPlayer()
	{
		GlobalVariableStorage.Party.Add(character);
		Debug.Log(GlobalVariableStorage.GetEncounterAsText());

		label.text = GlobalVariableStorage.GetEncounterAsText();
	}

	public void RemoveThisPlayer()
	{
		GlobalVariableStorage.Party.Remove(character);
		Debug.Log(GlobalVariableStorage.GetEncounterAsText());

		label.text = GlobalVariableStorage.GetEncounterAsText();
	}
}
