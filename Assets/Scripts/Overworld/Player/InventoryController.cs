using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static List<Item> items = new List<Item>();

    //Pull in items from sqlite
    public static void GetItemsFromDB()
    {
        IDbConnection conn = GlobalVariableStorage.CreateAndOpenDatabase();

        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = "SELECT * FROM items";

        IDataReader reader = comm.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("Receiving Item");

            items.Add(new Item
            {
                itemName = reader.GetString(0),
                quantity = reader.GetFloat(1),
                description = reader.GetString(2)
            });
        }

        conn.Close();
    }

    //Save Items to database
    public static void SaveItems()
    {
        IDbConnection conn = GlobalVariableStorage.CreateAndOpenDatabase();

        foreach (Item item in items)
        {
            SqliteParameter name = new SqliteParameter("$name", item.itemName);
            SqliteParameter qty = new SqliteParameter("$quantity", item.quantity);
            SqliteParameter description = new SqliteParameter("$description", item.description);
            
            IDbCommand comm = conn.CreateCommand();
            comm.CommandText = "INSERT OR REPLACE INTO items (name, quantity, description) VALUES ($name, $quantity, $description)";
            comm.Parameters.Add(name);
            comm.Parameters.Add(qty);
            comm.Parameters.Add(description);
            comm.ExecuteNonQuery();
        }

        conn.Close();
    }

    //Pass item from internal list based on request;
    public static Item GetItem(string itemName)
    {
        Item item = null;

        foreach (Item thing in items)
        {
            if (thing.itemName == itemName)
            {
                item = thing;
            }
        }

        return item;
    }

    public static void UseItem(string itemName)
    {
        Item item = null;

        foreach (Item thing in items)
        {
            if (thing.itemName == itemName)
            {
                item = thing;
            }
        }

        item.Use();

        if(item.quantity < 1)
        {
            items.Remove(item);
        }
    }

    public static void AddItem(Item item)
    {
        items.Add(item);
    }
}
