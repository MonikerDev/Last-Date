using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControllerTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InventoryController.GetItemsFromDB();
        List<Item> items = InventoryController.items;

        foreach(Item item in items)
        {
            Debug.Log("Name: " + item.itemName + " | Quantity: " + 
                item.quantity + " | Description: " + item.description);
        }

        InventoryController.AddItem(new Item
        {
            itemName = "Lore",
            quantity = 5,
            description = "can do it"
        });

        items[1].Use();

        InventoryController.SaveItems();
    }
}
