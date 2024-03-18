using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    //Pull in items from sqlite
    public void GetItemsFromDB()
    {
        IDbConnection conn = GlobalVariableStorage.CreateAndOpenDatabase();

        IDbCommand comm = conn.CreateCommand();
        comm.CommandText = "SELECT * FROM items";

        IDataReader reader = comm.ExecuteReader();

        while (reader.Read())
        {
            items.Add(new Item
            {
                name = reader.GetString(1),
                quantity = reader.GetInt32(1),
                description = reader.GetString(1)
            });
        }
    }

    //Save Items to database
    public void SaveItems()
    {

    }

    //Pass item from internal list based on request;
    public Item GetItem(string itemName)
    {
        Item item = null;

        foreach (Item thing in items)
        {
            if (thing.name == itemName)
            {
                item = thing;
            }
        }

        return item;
    }

    public void UseItem(string itemName)
    {
        Item item = null;

        foreach (Item thing in items)
        {
            if (thing.name == itemName)
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

    public void AddItem(Item item)
    {
        items.Add(item);
    }
}
