using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coverhandler : MonoBehaviour
{
	public PhysicsBasedPlayerController player;

	[Header("Hiding Modifiers")]
	public float hideDelay;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player" && !player.isHidden && 
			player.state == PhysicsBasedPlayerController.MovementState.crouching)
		{
			player.HideCharacter();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log("It continues");
		if (collision.gameObject.tag == "Player" && !player.isHidden &&
			player.state == PhysicsBasedPlayerController.MovementState.crouching)
		{
			Invoke(nameof(HidePlayer), hideDelay);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.tag == "Player" && player.isHidden)
		{
			player.UnHideCharacter();
		}
	}

	private void HidePlayer()
	{
		player.HideCharacter();
	}
}
