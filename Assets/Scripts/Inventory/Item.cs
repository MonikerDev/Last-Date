using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Item
{
	//Secret mouskatool for later
	public Sprite Sprite;

	//Necessities
	public string itemName;
	public string description;

	//Added for capstone requirements
	//May not be in final game
	//Intend to use yarn to manage
	//all flags, including pickups
	public float quantity;

	public Item() { }

	//use an item
	public void Use()
    {
		if(this.quantity > 0)
        {
			this.quantity -= 1;
        }
    }

	//NOT FINSIHED NEEDS TO SET NOT SELECT
	//Used for non-stackable items
	//where quantity does not matter
	public void TriggerFlag()
	{
		IDbConnection conn = GlobalVariableStorage.CreateAndOpenDatabase();

		SqliteParameter param = new SqliteParameter("$name", itemName);

		IDbCommand command = conn.CreateCommand();
		command.CommandText = "INSERT OR REPLACE INTO YarnBools (key, value) VALUES ($name, 1)";
		command.Parameters.Add(param);

		command.ExecuteNonQuery();
	}
}
