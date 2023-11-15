using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattleButton : MonoBehaviour
{
    public void StartBattle()
	{
        GlobalVariableStorage.AddToParty("Cynthia");
        GlobalVariableStorage.AddToParty("Ashe");
        GlobalVariableStorage.AddToParty("Emi");
        GlobalVariableStorage.AddToParty("Madi");
        GlobalVariableStorage.CompileEncounter();
        SceneManager.LoadScene("TurnBasedBattleArena");
    }
}
