using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TestTrigger : MonoBehaviour
{
	bool dialogStarted = false;
	public string dialogNode;

	Collider2D c2d;
	bool playerTouch = false;

	public GameObject DialogSystem;
	DialogueRunner dr;

	private void Start()
	{
		c2d = this.GetComponent<Collider2D>();
		dr =
		dr = DialogSystem.GetComponent<DialogueRunner>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
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

	public void EndDialog()
	{
		dialogStarted = false;
		PlayerController.canMove = true;
	}

	private void Update()
	{
		if (playerTouch && !dialogStarted && Input.GetKeyUp(KeyCode.Space))
		{
			PlayerController.canMove = false;
			dr.StartDialogue(dialogNode);
			dialogStarted = true;
			UnityEngine.Events.UnityEvent endDialog = new UnityEngine.Events.UnityEvent();
			endDialog.AddListener(EndDialog);
			dr.onDialogueComplete = endDialog;
		}
	}
}
