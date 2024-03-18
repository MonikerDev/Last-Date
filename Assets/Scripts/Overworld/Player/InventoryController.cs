using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    //Pull in items from sqlite
    public void GetItemsFromDB()
    {

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
