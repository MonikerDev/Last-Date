using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EncounterBuilder : MonoBehaviour
{
    public static List<string> characters = new List<string>();
    public TMP_Text characterList;

    public void PopulateList()
	{
        string text = "";

        foreach(string ch in characters)
		{
            text += ch + ", ";
		}

        characterList.text = text;
	}

    public void AddCharacter()
	{
        characters.Add(this.GetComponent<GameObject>().tag);
	}

    public void RemoveCharacter()
    {
        characters.Remove(this.GetComponent<GameObject>().tag);
    }
}
