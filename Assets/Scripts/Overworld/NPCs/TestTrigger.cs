using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
	Collider2D c2d;

	private void Start()
	{
		c2d = this.GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("triggered");
		if(collision.tag == "Player")
		{
			Debug.Log("player triggered");
		}
	}
}
