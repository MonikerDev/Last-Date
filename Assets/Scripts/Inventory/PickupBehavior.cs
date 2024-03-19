using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
	public string itemName;
	public Item item;
	bool playerHere = false;

	//Pull in item from database
    private void Start()
    {
		item = InventoryController.GetItem(itemName);
    }

	//Add item to inventory
    public void Pickup()
	{
		InventoryController.AddItem(item);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			playerHere = true;
			Debug.Log("Player here");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerHere = false;
			Debug.Log("Player leave me :(");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && playerHere)
		{
			Pickup();
			Debug.Log(item.itemName + " was picked up");
		}
	}
}
