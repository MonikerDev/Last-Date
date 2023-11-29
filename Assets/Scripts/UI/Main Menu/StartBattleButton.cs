using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattleButton : MonoBehaviour
{
    public void StartBattle()
	{
		if(GlobalVariableStorage.Party.Count > 0 && (GlobalVariableStorage.CurrentEncounter.Count + GlobalVariableStorage.Party.Count) >= (GlobalVariableStorage.Party.Count)){
			GlobalVariableStorage.CompileEncounter();
			SceneManager.LoadScene("TurnBasedBattleArena");
		}
		else
		{
			Debug.Log("Invalid Encounter");
		}
    }
}
