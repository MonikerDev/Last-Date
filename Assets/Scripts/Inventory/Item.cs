using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string Name;
	public Sprite Sprite;



	//NOT FINSIHED NEEDS TO SET NOT SELECT
	public void TriggerFlag()
	{
		IDbConnection conn = GlobalVariableStorage.CreateAndOpenDatabase();

		SqliteParameter param = new SqliteParameter("$name", Name);

		IDbCommand command = conn.CreateCommand();
		command.CommandText = "INSERT OR REPLACE INTO YarnBools (key, value) VALUES ($name, 1)";
		command.Parameters.Add(param);

		command.ExecuteNonQuery();
	}
}
