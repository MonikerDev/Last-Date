using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestBattletrigger : MonoBehaviour
{
	Collider2D c2d;
	bool playerTouch = false;

	private void Start()
	{
		c2d = this.GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			playerTouch = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			playerTouch = false;
		}
	}

	private void Update()
	{
		if (playerTouch && Input.GetKeyUp(KeyCode.Space))
		{
			PlayerController.canMove = false;
			GlobalVariableStorage.Party.Clear();

			List<string> debugParty = new List<string>();
			debugParty.Clear();
			debugParty.Add("Cynthia");
			debugParty.Add("Ashe");
			GlobalVariableStorage.Party.AddRange(debugParty);

			GlobalVariableStorage.CompileEncounter();

			GlobalVariableStorage.previousScene = SceneManager.GetActiveScene().name;
			Debug.Log(GlobalVariableStorage.previousScene);

			PlayerController.canMove = true;
			SceneManager.LoadScene("TurnBasedBattleArena");
		}
	}
}
